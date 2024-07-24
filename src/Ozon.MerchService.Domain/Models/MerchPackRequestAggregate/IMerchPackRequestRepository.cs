using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public interface IMerchPackRequestRepository : IRepository<MerchPackRequest, long>
{
    Task<IEnumerable<MerchPackRequest>> GetByRequestStatusAsync(RequestStatus requestStatus, CancellationToken cancellationToken);
    Task<IEnumerable<MerchPackRequest>> GetAllByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken);
}