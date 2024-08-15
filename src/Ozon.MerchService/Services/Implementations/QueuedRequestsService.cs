using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Services.Implementations;

/// <summary>
/// Implementation of service for reserve queued merch pack requests
/// </summary>
/// <param name="merchPackRequestRepository">Merch pack request repository</param>
/// <param name="mediator">Mediator instance</param>
/// <param name="logger">Logger instance</param>
public class QueuedRequestsService(
    IMerchPackRequestRepository merchPackRequestRepository,
    IMediator mediator,
    ILogger<QueuedRequestsService> logger) : IQueuedRequestsService
{
    /// <summary>
    /// Try to reserve queued merch pack requests
    /// </summary>
    /// <param name="skuCollection">Collection of skus that replenished on stock</param>
    /// <param name="token">Cancellation token</param>
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