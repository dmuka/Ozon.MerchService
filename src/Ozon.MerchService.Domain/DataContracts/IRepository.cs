using Npgsql;
using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Domain.DataContracts;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="TEntity">Type of entity</typeparam>
/// <typeparam name="TId">Entity id type</typeparam>
public interface IRepository
{
    /// <summary>
    /// Create new item
    /// </summary>
    /// <param name="item">New item</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <param name="parameters"></param>
    /// <returns>Created item id</returns>
    Task<TId> CreateAsync<TEntity, TId>(CancellationToken token, object parameters) 
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
    Task<int> UpdateAsync<T, TDto>(T itemToUpdate, CancellationToken cancellationToken) 
        where T : Entity
        where TDto : BaseEntity;
    
    /// <summary>
    /// Delete item
    /// </summary>
    /// <param name="itemId">Item to delete</param>
    /// <param name="token">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Affected rows count</returns>
    Task<int> DeleteAsync<T, TId>(TId itemId, CancellationToken token)
        where TId : IEquatable<TId>;
}