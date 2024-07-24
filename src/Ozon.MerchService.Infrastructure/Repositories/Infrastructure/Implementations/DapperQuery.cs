using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;

public class DapperQuery
{
    public class QueryExecutor(ITracker tracker) : IDapperQuery
    {
        public async Task<T> Call<T>(T entity, Func<Task> function) where T : Entity
        {
            await function();
            tracker.Track(entity);
            
            return entity;
        }

        public async Task<T> Call<T>(Func<Task<T>> function) where T : Entity
        {
            var result = await function();
            tracker.Track(result);
            
            return result;
        }

        public async Task<IEnumerable<T>> Call<T>(Func<Task<IEnumerable<T>>> function) where T : Entity
        {
            var result = (await function()).ToList();
            foreach (var entity in result)
            {
                tracker.Track(entity);
            }

            return result;
        }
    }
}