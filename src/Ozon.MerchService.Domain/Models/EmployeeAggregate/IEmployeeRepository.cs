using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> FindAsync(int id, CancellationToken token);
}