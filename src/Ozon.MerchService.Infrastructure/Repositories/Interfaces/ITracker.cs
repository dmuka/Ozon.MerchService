using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Tracker with entities changing in query 
/// </summary>
public interface ITracker<T> where T : IEquatable<T>
{
    /// <summary>
    /// Tracked entities in query
    /// </summary>
    IEnumerable<Entity<T>> TrackedEntities { get; }

    /// <summary>
    /// Add entity to track
    /// </summary>
    /// <param name="entity">Tracked entity</param>
    public void Track(Entity<T> entity);
}