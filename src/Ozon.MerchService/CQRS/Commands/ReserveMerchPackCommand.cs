using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Reserve merch pack command
/// </summary>
public class ReserveMerchPackCommand : IRequest<RequestStatus>
{
    /// <summary>
    /// Reserve merch pack command constructor
    /// </summary>
    /// <param name="merchPackRequest">Merch pack request instance</param>
    /// <param name="eventType">Employee event type instance</param>
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest, EmployeeEventType? eventType)
    {
        EventType = eventType;
        MerchPackRequest = merchPackRequest;
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