using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;

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
    
    /// <summary>
    /// Reserve merch pack command constructor
    /// </summary>
    /// <param name="merchPackRequest">Merch pack request instance</param>
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest)
    {
        MerchPackRequest = merchPackRequest;
    }

    /// <summary>
    /// Merch pack request property
    /// </summary>
    public MerchPackRequest MerchPackRequest { get; private set; }

    /// <summary>
    /// Employee event type property
    /// </summary>
    public EmployeeEventType? EventType { get; private set; }
}