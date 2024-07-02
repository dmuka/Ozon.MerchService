using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.DataContracts;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="TEntity">Объект сущности для управления</typeparam>
public interface IRepository<TEntity> where TEntity : Entity<long>
{
    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="item">New item</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Created item id</returns>
    Task<int> CreateAsync(TEntity item, CancellationToken token);

    /// <summary>
    /// Update item
    /// </summary>
    /// <param name="itemToUpdate">Item to update</param>
    /// <param name="cancellationToken">Cancellation token  <see cref="CancellationToken"/></param>
    /// <returns>Updated item id</returns>
    Task<int> UpdateAsync(TEntity itemToUpdate, CancellationToken cancellationToken);
}