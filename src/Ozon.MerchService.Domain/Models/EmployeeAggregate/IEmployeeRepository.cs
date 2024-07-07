using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public interface IEmployeeRepository : IRepository<Employee, long>
{
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken);
}