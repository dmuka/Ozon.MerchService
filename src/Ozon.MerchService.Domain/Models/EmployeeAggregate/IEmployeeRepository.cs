using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public interface IEmployeeRepository : IRepository<Employee, long>
{
    Task<Employee> FindAsync(int id, CancellationToken token);
}