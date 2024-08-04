using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Integration;
using Ozon.MerchService.Domain.Models.Extensions;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackCommandHandler(
            IRepository repository,
            IMerchPackRequestRepository merchPackRequestRepository,
            IMerchPacksRepository merchPacksRepository,
            IStockGrpcService stockGrpcService,
            IMessageBroker broker,
            IOptions<KafkaConfiguration> kafkaConfig,
            IUnitOfWork unitOfWork) : IRequestHandler<ReserveMerchPackCommand, RequestStatus>
    {
        private const string HandlerName = nameof(ReserveMerchPackCommandHandler);
        
        public async Task<RequestStatus> Handle(ReserveMerchPackCommand request, CancellationToken token)
        {
            await unitOfWork.StartTransaction(token);

            var canReceiveMerchPack = request.MerchPackRequest.Employee.CanReceiveMerchPack(request.MerchPackRequest.MerchPackType);

            if (!canReceiveMerchPack)
            {
                request.MerchPackRequest.SetStatusDeclined();
                
                return request.MerchPackRequest.RequestStatus;
            }
            
            stockGrpcService.SetItemsSkusInRequest(request.MerchPackRequest, request.MerchPackRequest.ClothingSize, token);

            var dto = new MerchPackRequestDto()
            {
                MerchpackTypeId = (int)request.MerchPackRequest.MerchPackType,
                MerchPackItems = JsonSerializer.Serialize(request.MerchPackRequest.MerchItems.Select(item => new
                {
                    item.Type.ItemTypeId,
                    item.Type.ItemTypeName,
                    item.Sku.Value
                })),
                EmployeeId = request.MerchPackRequest.Employee.Id,
                ClothingSizeId = (int)request.MerchPackRequest.ClothingSize,
                HrEmail = request.MerchPackRequest.HrEmail.Value,
                RequestTypeId = request.MerchPackRequest.RequestType.Id,
                RequestedAt = DateTimeOffset.UtcNow,
                RequestStatusId = request.MerchPackRequest.RequestStatus.Id
            };
            
            var merchPackRequestId = request.MerchPackRequest.RequestStatus.Is(RequestStatus.Created)
                ? await repository.CreateAsync<MerchPackRequestDto, long>(token, dto)
                : request.MerchPackRequest.Id;
            
            if (!canReceiveMerchPack)
            {
                request.MerchPackRequest.SetStatusDeclined();
                
                await repository.UpdateAsync(dto, token);
                
                return request.MerchPackRequest.RequestStatus;
            }
            
            if (await stockGrpcService.TryReserveMerchPackItems(request.MerchPackRequest.MerchItems, token))
            {
                if (request.MerchPackRequest.RequestStatus.Is(RequestStatus.Queued))
                {
                    var employeeNotificationEvent = new MerchReplenishedEvent(
                        request.MerchPackRequest.Employee.Email, request.MerchPackRequest.MerchPackType);

                    await broker.ProduceAsync(
                        kafkaConfig.Value.EmployeeNotificationEventTopic, 
                        request.MerchPackRequest.Id.ToString(), 
                        employeeNotificationEvent, 
                        token);
                }
                
                request.MerchPackRequest.SetStatusIssued();
            }
            else
            {
                request.MerchPackRequest.SetStatusQueued();
                
                var hrNotificationEvent = new MerchEndedEvent(request.MerchPackRequest.HrEmail, request.MerchPackRequest.MerchPackType);

                await broker.ProduceAsync(
                    kafkaConfig.Value.EmployeeNotificationEventTopic, 
                    request.MerchPackRequest.Id.ToString(),
                    hrNotificationEvent,
                    token);
            }
            
            var affectedRows = await merchPackRequestRepository.UpdateAsync(request.MerchPackRequest, token);

            await unitOfWork.SaveChangesAsync(token);
            
            return request.MerchPackRequest.RequestStatus;
        }
    }