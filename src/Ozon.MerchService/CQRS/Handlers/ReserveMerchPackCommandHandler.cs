using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackCommandHandler(
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService,
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
                
                var affRows = await merchPackRequestRepository
                    .UpdateAsync<MerchPackRequest, MerchPackRequestDto>(request.MerchPackRequest, token);
                
                return (request.MerchPackRequest.RequestStatus, affRows);
            }

            var merchPackAvailable =
                await stockGrpcService.TryReserveMerchPackItems(request.MerchPackRequest.MerchPack.Items, token);
            
            if (merchPackAvailable)
            {
                request.MerchPackRequest.ReserveMerchPack();
            }
            else
            {
                request.MerchPackRequest.QueueMerchPack();
            }
            
            var affectedRows = await merchPackRequestRepository
                .UpdateAsync<MerchPackRequest, MerchPackRequestDto>(request.MerchPackRequest, token);

            await unitOfWork.SaveChangesAsync(token);
            
            return (request.MerchPackRequest.RequestStatus, affectedRows);
        }
    }