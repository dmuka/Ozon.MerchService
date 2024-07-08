using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

/// <summary>
/// Tracker with entities changing in query 
/// </summary>
public interface ITracker<TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// Tracked entities in query
    /// </summary>
    IEnumerable<Entity<TId>> TrackedEntities { get; }

    /// <summary>
    /// Add entity to track
    /// </summary>
    /// <param name="entity">Tracked entity</param>
    public void Track(Entity<TId> entity);
}