using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class EmployeesRepository(IUnitOfWork unitOfWork) : Repository<Employee, long>(unitOfWork), IEmployeeRepository
{
}