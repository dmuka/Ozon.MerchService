using Ozon.MerchService.Domain.Aggregates;

namespace Ozon.MerchService.Domain.DataContracts;

/// <summary>
/// Base repository interface
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Get all items of type <see cref="T"/>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">Items type</typeparam>
    /// <returns>All items of type <see cref="T"/></returns>
    Task<IEnumerable<T>> GetAllAsync<T>(CancellationToken cancellationToken);
    
    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <param name="parameters"></param>
    /// <returns>Created item id</returns>
    Task<TId> CreateAsync<TEntity, TId>(object parameters, CancellationToken token) 
        where TId : IEquatable<TId>;
    
    /// <summary>
    /// Get item by id
    /// </summary>
    /// <param name="itemId">Item id</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Item with related id</returns>
    Task<TEntity> GetByIdAsync<TEntity, TId>(TId itemId, CancellationToken token) 
        where TId : IEquatable<TId>;
    
    /// <summary>
    /// Update item
    /// </summary>
    /// <param name="itemToUpdate">Item to update</param>
    /// <param name="cancellationToken">Cancellation token  <see cref="CancellationToken"/></param>
    /// <returns>Affected rows count</returns>
    Task<int> UpdateAsync<T>(T itemToUpdate, CancellationToken cancellationToken) 
        where T : Entity;
    
    /// <summary>
    /// Delete item
    /// </summary>
    /// <param name="itemId">Item to delete</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Affected rows count</returns>
    Task<int> DeleteAsync<T, TId>(TId itemId, CancellationToken token)
        where TId : IEquatable<TId>;
}