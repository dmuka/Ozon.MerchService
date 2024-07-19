using System.Collections.Concurrent;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;

public class Tracker : ITracker 
{
    public IEnumerable<Entity> TrackedEntities => _trackedEntities.ToArray();

    public void Track(Entity entity)
    {
        _trackedEntities.Add(entity);
    }

    private readonly ConcurrentBag<Entity> _trackedEntities = [];
}