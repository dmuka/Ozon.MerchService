using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Services.Implementations;

/// <summary>
/// Merch service
/// </summary>
public class MerchService : IMerchService
{
    private List<MerchPack> MerchPacks { get; } =
    [
        new MerchPack(MerchType.ProbationPeriodEndingPack),
        new MerchPack(MerchType.WelcomePack)
    ];

    private MerchPack MerchPack => new (MerchType.ConferenceListenerPack);

    /// <summary>
    /// Get asynchronously information about merch pack(s) that user already get
    /// </summary>
    /// <param name="employeeId">Employee id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Received merch response</returns>
    public async Task<List<MerchPack>> GetReceivedMerchAsync(
        long employeeId, 
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(MerchPacks);
    }

    /// <summary>
    /// Reserve merch for employee
    /// </summary>
    /// <param name="merchPack">Merch pack</param>
    /// <param name="employeeId">Employee id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reserved merch pack</returns>
    public async Task<MerchPack> ReserveMerchAsync(
        long employeeId,
        MerchPack merchPack,
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(merchPack);
    }
}