using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class Employee : Entity<long>
{
    public List<MerchPack> MerchPacks { get; } = [];
}