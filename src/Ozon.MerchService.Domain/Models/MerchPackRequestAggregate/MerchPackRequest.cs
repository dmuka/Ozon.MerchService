using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public class MerchPackRequest : Entity<long>, IAggregationRoot
{
    public MerchPackRequest(MerchType merchPackType, Employee employee)
    {
        MerchPackType = merchPackType;
        MerchItems = new MerchPack(MerchPackType, employee.ClothingSize).Items;
        Employee = employee;
        Status = Status.Created;
    }

    public MerchType MerchPackType { get; }

    public IReadOnlyCollection<MerchItem> MerchItems { get; }

    public Employee Employee { get; }

    public RequestedAt RequestedAt { get; } = new();

    public Issued Issued { get; } = new();

    public Status Status { get; private set; }

    public static MerchPackRequest CreateInstance(long id, MerchPackRequest merchPackRequest)
    {
        merchPackRequest.Id = id;

        return merchPackRequest;
    }

    public bool CheckMerchItemsReserveOnStock()
    {
        var result = MerchItems.All(item => item.Reserved.Value);

        return result;
    }

    public void SetStatus(Status status)
    {
        Status = status;
    }
}