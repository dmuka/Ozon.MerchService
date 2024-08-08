using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

public interface IDapperQuery
{
    Task<T> Call<T>(T entity, Func<Task> function) where T : Entity;
    Task<T> Call<T>(Func<Task<T>> function) where T : Entity;
    Task<int> CallAction<T>(T entity, Func<Task<int>> function) where T : Entity;
    Task<IEnumerable<T>> Call<T>(Func<Task<IEnumerable<T>>> function) where T : Entity;
}