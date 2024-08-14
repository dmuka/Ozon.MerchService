using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;

public interface IEmployeeRepository : IRepository
{
    //Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Get item by email
    /// </summary>
    /// <param name="email">Employee email</param>
    /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
    /// <returns>Item with related email</returns>
    Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken);
}