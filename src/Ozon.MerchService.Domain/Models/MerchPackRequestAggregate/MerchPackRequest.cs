using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

[Table("merchpack_requests")]
public class MerchPackRequest : Entity, IAggregationRoot
{
    public MerchPackRequest(
        Employee employee,
        MerchPack merchPack,
        ClothingSize clothingSize,
        Email hrEmail,
        RequestType requestType,
        DateTimeOffset requestedAt,
        DateTimeOffset issued,
        RequestStatus requestStatus)
    {
        Employee = employee;
        MerchPack = merchPack;
        MerchPackType = merchPack.MerchPackType;
        HrEmail = hrEmail;
        ClothingSize = clothingSize;
        RequestType = requestType;
        RequestedAt = new RequestedAt(requestedAt);
        Issued = new Issued(issued);
        RequestStatus = requestStatus;
    }
    
    public MerchPackRequest(
        MerchPack merchPack,
        IEnumerable<MerchItem> merchItems,
        Employee employee, 
        RequestType requestType)
    {
        MerchPackType = merchPack.MerchPackType;
        _merchPackItems = merchPack.Items.ToList();
        Employee = employee;
        RequestStatus = RequestStatus.Created;
        RequestType = requestType;
    }
    
    public MerchPackRequest(
        MerchType merchPackType,
        Employee employee, 
        RequestType requestType)
    {
        MerchPackType = merchPackType;
        Employee = employee;
        RequestStatus = RequestStatus.Created;
        RequestType = requestType;
    }

    [Column("merchpack_id")]
    public MerchType MerchPackType { get; }

    private List<MerchItem> _merchPackItems;
    
    public IEnumerable<MerchItem> MerchItems => _merchPackItems.AsReadOnly();
    
    public MerchPack MerchPack { get; }

    public Employee Employee { get; }

    public Email HrEmail { get; }
    
    public ClothingSize ClothingSize { get; }

    public RequestedAt RequestedAt { get; } = new();

    public Issued Issued { get; } = new();

    public RequestStatus RequestStatus { get; private set; }
    
    public RequestType RequestType { get; private set; }

    // public static MerchPackRequest CreateInstance(long id, MerchPackRequest merchPackRequest)
    // {
    //     merchPackRequest.Id = id;
    //
    //     return merchPackRequest;
    // }

    public static MerchPackRequest CreateInstance(
        long merchPackRequestId,
        MerchType merchPackType,
        MerchItem[] merchPackItems,
        long employeeId,
        string employeeFullName,
        string employeeEmail,
        ClothingSize clothingSize,
        string hrEmail,
        int requestTypeId,
        string requestTypeName,
        DateTimeOffset requestedAt,
        DateTimeOffset issued,
        int statusId,
        string statusName)
    {
        var items = merchPackItems;
        
        var merchPackRequest = new MerchPackRequest(
            Employee.CreateInstance(employeeId, employeeFullName, employeeEmail),
            new MerchPack(merchPackType, items, clothingSize),
            clothingSize,
            new Email(hrEmail),
            new RequestType(requestTypeId, requestTypeName),
            requestedAt,
            issued,
            new RequestStatus(statusId, statusName)
            )
        {
            Id = merchPackRequestId,
            _merchPackItems = merchPackItems.ToList()
        };

        return merchPackRequest;
    }

    public bool CheckMerchItemsReserveOnStock()
    {
        var result = MerchItems.All(item => item.Reserved.Value);

        return result;
    }

    public void SetStatusDeclined()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        RequestStatus = RequestStatus.Declined;
    }

    public void SetStatusQueued()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (RequestStatus.Equals(RequestStatus.Queued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already quequed ({Employee.FullName})");
        }
        
        RequestStatus = RequestStatus.Queued;
    }

    public void SetStatusIssued()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (RequestStatus.Equals(RequestStatus.Declined))
        {
            throw new ArgumentException($"Merch {MerchPackType} declined to {Employee.FullName}");
        }
        
        RequestStatus = RequestStatus.Issued;
    }
}