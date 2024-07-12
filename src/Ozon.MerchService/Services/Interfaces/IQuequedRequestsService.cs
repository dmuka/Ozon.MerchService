using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.Services.Interfaces;

public interface IQuequedRequestsService
{
    Task RepeatReserve(IEnumerable<long> skuCollection, CancellationToken token);
}