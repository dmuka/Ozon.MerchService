using Npgsql;

namespace Ozon.MerchService.Domain.DataContracts;

/// <summary>
/// Unit of work interface
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Start transaction
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns></returns>
    ValueTask StartTransaction(CancellationToken token);
    
    /// <summary>
    /// Save all changes in transaction to database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}