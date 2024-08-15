namespace Ozon.MerchService.Services.Interfaces;

/// <summary>
/// Service for reserve queued merch pack requests
/// </summary>
public interface IQueuedRequestsService
{
    /// <summary>
    /// Try to reserve queued merch pack requests
    /// </summary>
    /// <param name="skuCollection">Collection of skus that replenished on stock</param>
    /// <param name="token">Cancellation token</param>
    Task RepeatReserve(IEnumerable<long> skuCollection, CancellationToken token);
}