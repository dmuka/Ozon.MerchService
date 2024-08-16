using System.ComponentModel.DataAnnotations.Schema;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Events.Integration;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Domain.Models.Extensions;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;

public class MerchPackRequest : Entity, IAggregationRoot
{
    private const int EnumerationIdMinimumValue = 1;
    
    public MerchPackRequest(
        Employee employee,
        MerchPack merchPack,
        ClothingSize clothingSize,
        Email hrEmail,
        RequestType requestType,
        DateTimeOffset requestedAt,
        RequestStatus requestStatus)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        MerchPack = merchPack ?? throw new ArgumentNullException(nameof(merchPack));
        HrEmail = hrEmail ?? throw new ArgumentNullException(nameof(hrEmail));
        ClothingSize = clothingSize;
        RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
        RequestedAt = new RequestedAt(requestedAt);
        RequestStatus = requestStatus ?? throw new ArgumentNullException(nameof(requestStatus));
    }
    
    public MerchPackRequest(
        MerchType merchPackType,
        ClothingSize clothingSize,
        Employee employee,
        Email hrEmail,
        RequestType requestType,
        RequestStatus requestStatus)
    {
        MerchPackType = merchPackType;
        ClothingSize = clothingSize;
        Employee = employee;
        HrEmail = hrEmail;
        RequestStatus = requestStatus;
        RequestType = requestType;
    }

    public MerchType MerchPackType { get; }
    
    public MerchPack MerchPack { get; private set; }

    public Employee Employee { get; private set; }

    public Email HrEmail { get; private set; }
    
    public ClothingSize ClothingSize { get; private set; }

    public RequestedAt RequestedAt { get; private set; }

    public Issued Issued { get; private set; }

    public RequestStatus RequestStatus { get; private set; }
    
    public RequestType RequestType { get; private set; }

    public static MerchPackRequest CreateInstance(
        long id, 
        Employee employee, 
        MerchPack merchPack,
        Email hrEmail,
        ClothingSize clothingSize,
        RequestType requestType)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

        var request = new MerchPackRequest(
            employee,
            merchPack,
            clothingSize,
            hrEmail,
            requestType,
            DateTimeOffset.UtcNow,
            RequestStatus.Created)
        {
            Id = id
        };

        return request;
    }

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
        DateTimeOffset? issued,
        int statusId,
        string statusName)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(merchPackRequestId, nameof(merchPackRequestId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(employeeId, nameof(employeeId));
        ArgumentOutOfRangeException.ThrowIfLessThan(requestTypeId, EnumerationIdMinimumValue, nameof(requestTypeId));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(requestTypeId, Enumeration.GetAll<RequestType>().Count(), nameof(requestTypeId));
        ArgumentOutOfRangeException.ThrowIfLessThan(statusId, EnumerationIdMinimumValue, nameof(statusId));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(statusId, Enumeration.GetAll<RequestStatus>().Count(), nameof(statusId));
        
        var merchPackRequest = new MerchPackRequest(
            Employee.CreateInstance(employeeId, employeeFullName, string.Empty, employeeEmail),
            new MerchPack(merchPackType, merchPackItems),
            clothingSize,
            new Email(hrEmail),
            new RequestType(requestTypeId, requestTypeName),
            requestedAt,
            new RequestStatus(statusId, statusName)
            )
        {
            Id = merchPackRequestId
        };

        return merchPackRequest;
    }

    public void Reserve()
    {
        SetStatusIssued();
        
        if (RequestStatus.Is(RequestStatus.Issued))
        {
            AddMerchPackIsReadyIntegrationEvent();
        }
    }    
    
    public void Queue()
    {
        SetStatusQueued();

        if (RequestStatus.Is(RequestStatus.Queued))
        {
            AddMerchEndedIntegrationEvent();
        }
    }

    private void AddMerchPackIsReadyIntegrationEvent()
    {
        var @event = new MerchPackIsReadyIntegrationEvent(Employee.Email, MerchPack.MerchPackType);
        
        AddDomainEvent(@event);
    }

    private void AddMerchEndedIntegrationEvent()
    {
        var @event = new MerchEndedEvent(HrEmail, MerchPack.MerchPackType);
        
        AddDomainEvent(@event);
    }

    public void SetStatusDeclined()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPack.MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        RequestStatus = RequestStatus.Declined;
    }

    public void SetStatusQueued()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPack.MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (RequestStatus.Equals(RequestStatus.Queued))
        {
            throw new ArgumentException($"Merch {MerchPack.MerchPackType} already quequed ({Employee.FullName})");
        }
        
        RequestStatus = RequestStatus.Queued;
    }

    public void SetStatusIssued()
    {
        if (RequestStatus.Equals(RequestStatus.Issued))
        {
            throw new ArgumentException($"Merch {MerchPack.MerchPackType} already issued {Issued} to {Employee.FullName}");
        }
        
        if (RequestStatus.Equals(RequestStatus.Declined))
        {
            throw new ArgumentException($"Merch {MerchPack.MerchPackType} declined to {Employee.FullName}");
        }
        
        RequestStatus = RequestStatus.Issued;
        Issued = new Issued(DateTimeOffset.UtcNow);
    }
}