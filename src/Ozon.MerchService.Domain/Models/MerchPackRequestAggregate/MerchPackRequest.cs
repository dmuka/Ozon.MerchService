using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public class MerchPackRequest : Entity<long>, IAggregationRoot
{
    public MerchPackRequest(
        MerchType merchPackType, 
        Employee employee, 
        RequestType requestType)
    {
        MerchPackType = merchPackType;
        _merchPackItems = new MerchPack(MerchPackType, employee.ClothingSize).Items.ToList();
        Employee = employee;
        Status = Status.Created;
        RequestType = requestType;
    }

    public MerchType MerchPackType { get; }

    private List<MerchItem> _merchPackItems;
    public IEnumerable<MerchItem> MerchItems => _merchPackItems.AsReadOnly();

    public Employee Employee { get; }

    public RequestedAt RequestedAt { get; } = new();

    public Issued Issued { get; } = new();

    public Status Status { get; private set; }

    public RequestType RequestType { get; private set; }

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

    public void SetStatusDeclined()
    {
        if (Status.Equals(Status.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        Status = Status.Declined;
    }

    public void SetStatusQuequed()
    {
        if (Status.Equals(Status.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (Status.Equals(Status.Quequed))
        {
            throw new ArgumentException($"Merch {MerchPackType} already quequed ({Employee.FullName})");
        }
        
        Status = Status.Quequed;
    }

    public void SetStatusIssued()
    {
        if (Status.Equals(Status.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (Status.Equals(Status.Declined))
        {
            throw new ArgumentException($"Merch {MerchPackType} declined to {Employee.FullName}");
        }
        
        Status = Status.Issued;
    }
}