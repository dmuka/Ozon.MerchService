using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Reserve merch pack command
/// </summary>
public class ReserveMerchPackCommand : IRequest<RequestStatus>
{
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest, EmployeeEventType? eventType)
    {
        EventType = eventType;
        MerchPackRequest = merchPackRequest;
    }
    
    public ReserveMerchPackCommand(
        string employeeFirstName,
        string employeeLastName,
        string employeeEmail,
        string hrEmail,
        string hrName,
        MerchType merchPackType,
        ClothingSize clothingSize)
    {
        Employee = new Employee( new FullName(employeeFirstName, employeeLastName), new Email(employeeEmail));
        MerchPackRequest = new MerchPackRequest(
            Employee, 
            new MerchPack(merchPackType, Array.Empty<MerchItem>()),
            clothingSize,
            new Email(hrEmail),
            RequestType.Manual,
            DateTimeOffset.UtcNow,
            RequestStatus.Created
            );
    }
    
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest)
    {
        MerchPackRequest = merchPackRequest;
    }

    public MerchPackRequest MerchPackRequest { get; private set; }
    /// <summary>
    /// Employee
    /// </summary>
    public Employee Employee { get; private set; }

    public EmployeeEventType? EventType { get; private set; }
}