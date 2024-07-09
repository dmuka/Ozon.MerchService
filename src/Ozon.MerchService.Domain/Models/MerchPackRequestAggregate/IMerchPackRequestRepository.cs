using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public interface IMerchPackRequestRepository : IRepository<MerchPackRequest, long>
{
    Task<IEnumerable<MerchPackRequest>> GetAllAsync(CancellationToken cancellationToken);
}