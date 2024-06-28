using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;
using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public class MerchPackRequest : Entity<long>, IAggregationRoot
{
    public MerchPackRequest(MerchType merchPackType, int employeeId)
    {
        MerchPackType = merchPackType;
        MerchItems = new MerchPack(MerchPackType).Items;
        EmployeeId = employeeId;
        Status = Status.Created;
    }

    public MerchType MerchPackType { get; }

    public IReadOnlyCollection<MerchItem> MerchItems { get; }

    public int EmployeeId { get; }

    public RequestedAt RequestedAt { get; } = new();

    public Issued Issued { get; } = new();

    public Status Status { get; }

    public bool CheckMerchItemsReserveOnStock()
    {
        var result = MerchItems.All(item => item.Reserved.Value);

        return result;
    }
}