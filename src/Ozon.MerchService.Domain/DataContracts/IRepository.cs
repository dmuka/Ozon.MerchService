using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.DataContracts;

public interface IRepository<TEntity> where TEntity : Entity<long>
{
}