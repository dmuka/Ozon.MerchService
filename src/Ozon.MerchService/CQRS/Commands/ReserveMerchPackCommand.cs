using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Reserve merch pack command
/// </summary>
public class ReserveMerchPackCommand : IRequest<RequestStatus>
{
    public ReserveMerchPackCommand(
        ReserveMerchRequest request, 
        EmployeeEventType eventType, 
        RequestStatus requestStatus, 
        RequestType requestType)
    {
        EventType = eventType;
        Employee = Employee.CreateInstance(
            request.EmployeeId, 
            request.EmployeeFirstName, 
            request.EmployeeLastName, 
            request.EmployeeEmail);
        MerchPackRequest = new MerchPackRequest(
            request.MerchPackType, 
            request.ClothingSize, 
            Employee, 
            new Email(request.HrEmail),
            requestType, 
            requestStatus);
    }
    
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest, EmployeeEventType eventType)
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