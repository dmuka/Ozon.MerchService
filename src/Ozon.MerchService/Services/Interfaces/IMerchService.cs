using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.Services.Interfaces;

/// <summary>
/// Interface for merch service
/// </summary>
public interface IMerchService
{
    /// <summary>
    /// Get asynchronously information about merch pack(s) that employee already receive
    /// </summary>
    /// <param name="employeeId">Employee id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Received merch response object</returns>
    Task<List<MerchPack>> GetReceivedMerchAsync(long employeeId, CancellationToken cancellationToken);

    /// <summary>
    /// Reserve merch for employee
    /// </summary>
    /// <param name="merchPack">Merch pack</param>
    /// <param name="employeeId">Employee id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reserved merch pack</returns>
    Task<MerchPack> ReserveMerchAsync(
        long employeeId,
        MerchPack merchPack,
        CancellationToken cancellationToken);
}