using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Services.Implementations;

public class QueuedRequestsService(
    IMerchPackRequestRepository merchPackRequestRepository,
    IMediator mediator,
    ILogger<QueuedRequestsService> logger) : IQueuedRequestsService
{
    public async Task RepeatReserve(IEnumerable<long> skuCollection, CancellationToken token)
    {
        var queuedMerchPackRequests = await merchPackRequestRepository.GetByRequestStatusAsync(RequestStatus.Queued, token);

        var replenishedMerchPackRequests = queuedMerchPackRequests
            .Where(request => request.MerchPack.Items.Any(item => skuCollection.Contains(item.Sku.Value)));
        
        foreach (var request in replenishedMerchPackRequests)
        {
            var command = new ReserveMerchPackCommand(request);
            
            await mediator.Send(command, token);
        }
    }
}