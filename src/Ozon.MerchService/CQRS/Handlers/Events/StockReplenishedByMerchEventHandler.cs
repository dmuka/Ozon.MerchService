using MediatR;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.BackgroundServices;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers.Events;

/// <summary>
/// Stock replenished by merch event handler
/// </summary>
/// <param name="queuedRequestsService">Queued requests service instance</param>
/// <param name="unitOfWork">Unit of work instance</param>
public class StockReplenishedByMerchEventHandler(IQueuedRequestsService queuedRequestsService, IUnitOfWork unitOfWork)
    : INotificationHandler<StockReplenishedByMerchEvent>
{
    /// <summary>
    /// Handle for stock replenished by merch event handler
    /// </summary>
    /// <param name="notification">Stock replenished by merch event instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Handle(StockReplenishedByMerchEvent notification, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);
        
        var skus = notification.Items.Select(item => item.Sku);
        
        await queuedRequestsService.RepeatReserve(skus, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}