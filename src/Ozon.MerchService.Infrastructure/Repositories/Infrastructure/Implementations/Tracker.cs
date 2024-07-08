using System.Collections.Concurrent;
using Ozon.MerchService.Domain.Root;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;

public class Tracker<T> : ITracker<T> 
    where T : IEquatable<T>
{
    public IEnumerable<Entity<T>> TrackedEntities => _trackedEntities.ToArray();

    public void Track(Entity<T> entity)
    {
        _trackedEntities.Add(entity);
    }

    private readonly ConcurrentBag<Entity<T>> _trackedEntities = [];
}