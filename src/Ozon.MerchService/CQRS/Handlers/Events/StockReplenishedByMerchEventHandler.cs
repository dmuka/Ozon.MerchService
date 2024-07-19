using MediatR;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.BackgroundServices;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class StockReplenishedByMerchEventHandler(IQueuedRequestsService queuedRequestsService, IUnitOfWork unitOfWork)
    : INotificationHandler<StockReplenishedByMerchEvent>
{
    public async Task Handle(StockReplenishedByMerchEvent notification, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);
        
        var skus = notification.Items.Select(item => item.Sku);
        
        await queuedRequestsService.RepeatReserve(skus, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}