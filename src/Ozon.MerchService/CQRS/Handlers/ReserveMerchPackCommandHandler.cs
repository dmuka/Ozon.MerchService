using AutoMapper;
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
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService,
            IMessageBroker broker,
            IMapper mapper,
            IOptions<KafkaConfiguration> kafkaConfig,
            IUnitOfWork unitOfWork) : IRequestHandler<ReserveMerchPackCommand, (RequestStatus status, int affectedRows)>
    {
        private const string HandlerName = nameof(ReserveMerchPackCommandHandler);
        
        public async Task<(RequestStatus status, int affectedRows)> Handle(ReserveMerchPackCommand request, CancellationToken token)
        {
            await unitOfWork.StartTransaction(token);

            var canReceiveMerchPack = request.MerchPackRequest.Employee.CanReceiveMerchPack(request.MerchPackRequest.MerchPack.MerchPackType);

            if (!canReceiveMerchPack)
            {
                request.MerchPackRequest.SetStatusDeclined();

                var declinedDto = mapper.Map<MerchPackRequestDto>(request.MerchPackRequest);
                
                var affRows = await merchPackRequestRepository.UpdateAsync(declinedDto, token, new { declinedDto.Id });
                
                return (request.MerchPackRequest.RequestStatus, affRows);
            }
            
            if (await stockGrpcService.TryReserveMerchPackItems(request.MerchPackRequest.MerchPack.Items, token))
            {
                if (request.MerchPackRequest.RequestStatus.Is(RequestStatus.Queued))
                {
                    var employeeNotificationEvent = new MerchReplenishedEvent(
                        request.MerchPackRequest.Employee.Email, request.MerchPackRequest.MerchPack.MerchPackType);

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
                
                var hrNotificationEvent = new MerchEndedEvent(request.MerchPackRequest.HrEmail, request.MerchPackRequest.MerchPack.MerchPackType);

                await broker.ProduceAsync(
                    kafkaConfig.Value.EmployeeNotificationEventTopic, 
                    request.MerchPackRequest.Id.ToString(),
                    hrNotificationEvent,
                    token);
            }
            
            var dto = mapper.Map<MerchPackRequestDto>(request.MerchPackRequest);
            
            var affectedRows = await merchPackRequestRepository.UpdateAsync(dto, token, new { dto.Id });

            await unitOfWork.SaveChangesAsync(token);
            
            return (request.MerchPackRequest.RequestStatus, affectedRows);
        }
    }