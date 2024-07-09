using Npgsql;
using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Domain.DataContracts;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="TEntity">Type of entity</typeparam>
/// <typeparam name="TId">Entity id type</typeparam>
public interface IRepository<TEntity, TId> 
    where TEntity : Entity<TId>, IAggregationRoot 
    where TId : IEquatable<TId>
{
    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="item">New item</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Created item id</returns>
    Task<TId> CreateAsync(TEntity item, CancellationToken token);
    
    /// <summary>
    /// Get item by id
    /// </summary>
    /// <param name="itemId">Item id</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Item with related id</returns>
    Task<TEntity> GetByIdAsync(TId itemId, CancellationToken token);
    
    /// <summary>
    /// Update item
    /// </summary>
    /// <param name="itemToUpdate">Item to update</param>
    /// <param name="cancellationToken">Cancellation token  <see cref="CancellationToken"/></param>
    /// <returns>Affected rows count</returns>
    Task<int> UpdateAsync(TEntity itemToUpdate, CancellationToken cancellationToken);
    
    /// <summary>
    /// Delete item
    /// </summary>
    /// <param name="itemId">Item to delete</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Affected rows count</returns>
    Task<int> DeleteAsync(TId itemId, CancellationToken token);
}