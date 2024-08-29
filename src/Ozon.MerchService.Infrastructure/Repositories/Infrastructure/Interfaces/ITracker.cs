using Ozon.MerchService.Domain.Aggregates;
using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

/// <summary>
/// Tracker with entities changing in query 
/// </summary>
public interface ITracker
{
    /// <summary>
    /// Tracked entities in query
    /// </summary>
    IEnumerable<Entity> TrackedEntities { get; }

    /// <summary>
    /// Add entity to track
    /// </summary>
    /// <param name="entity">Tracked entity</param>
    public void Track(Entity entity);
}