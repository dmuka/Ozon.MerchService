using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    /// <summary>
    /// Handler for reserving merch pack
    /// </summary>
    /// <param name="merchPackRequestRepository">Repository of merch pack requests</param>
    /// <param name="stockGrpcService">Stock grpc service</param>
    /// <param name="unitOfWork">Unit of work</param>
    public class ReserveMerchPackCommandHandler(
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService,
            IUnitOfWork unitOfWork) : IRequestHandler<ReserveMerchPackCommand, RequestStatus>
    {
        /// <summary>
        /// Handle for reserve merch pack command
        /// </summary>
        /// <param name="command">Reserve merch pack command</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public async Task<RequestStatus> Handle(ReserveMerchPackCommand command, CancellationToken token)
        {
            await unitOfWork.StartTransaction(token);

            var canReceiveMerchPack = command.MerchPackRequest.Employee.CanReceiveMerchPack(command.MerchPackRequest.MerchPack.MerchPackType);

            if (!canReceiveMerchPack)
            {
                command.MerchPackRequest.SetStatusDeclined();
                
                var affRows = await merchPackRequestRepository.UpdateAsync(command.MerchPackRequest, token);
                
                await unitOfWork.SaveChangesAsync(token);
                
                return command.MerchPackRequest.RequestStatus;
            }

            var merchPackAvailable =
                await stockGrpcService.TryReserveMerchPackItems(command.MerchPackRequest.MerchPack.Items, token);
            
            if (merchPackAvailable)
            {
                command.MerchPackRequest.ReserveMerchPack();
            }
            else
            {
                command.MerchPackRequest.QueueMerchPack();
            }
            
            var affectedRows = await merchPackRequestRepository.UpdateAsync(command.MerchPackRequest, token);

            await unitOfWork.SaveChangesAsync(token);
            
            return command.MerchPackRequest.RequestStatus;
        }
    }