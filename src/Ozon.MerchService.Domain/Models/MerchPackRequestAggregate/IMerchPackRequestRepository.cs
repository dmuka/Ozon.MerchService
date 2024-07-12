using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public interface IMerchPackRequestRepository : IRepository<MerchPackRequest, long>
{
    Task<IEnumerable<MerchPackRequest>> GetByRequestStatusAsync(Status status, CancellationToken cancellationToken);
}