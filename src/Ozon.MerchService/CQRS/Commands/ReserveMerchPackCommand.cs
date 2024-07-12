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
public class ReserveMerchPackCommand : IRequest<Status>
{
    public ReserveMerchPackCommand(
        ReserveMerchRequest request, 
        EmployeeEventType eventType, 
        Status status, 
        RequestType requestType)
    {
        EmployeeId = request.EmployeeId;
        EventType = eventType;
        EmployeeFullName = new FullName(request.EmployeeFirstName, request.EmployeeLastName).ToString();
        EmployeeEmail = request.EmployeeEmail;
        HrEmail = request.HrEmail;
        EmployeeClothingSize = request.ClothingSize;
        MerchPackType = request.MerchPackType;
        Status = status;
        RequestType = requestType;
    }
    
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest, EmployeeEventType eventType)
    {
        EmployeeId = merchPackRequest.Employee.Id;
        EventType = eventType;
        EmployeeFullName = merchPackRequest.Employee.FullName.ToString();
        EmployeeEmail = merchPackRequest.Employee.Email.ToString();
        HrEmail = merchPackRequest.Employee.HrEmail.ToString();
        EmployeeClothingSize = merchPackRequest.Employee.ClothingSize;
        MerchPackType = merchPackRequest.MerchPackType;
        Status = merchPackRequest.Status;
        RequestType = merchPackRequest.RequestType;
    }
    
    public ReserveMerchPackCommand(MerchPackRequest merchPackRequest)
    {
        Id = merchPackRequest.Id;
        Employee = merchPackRequest.Employee;
        MerchPackType = merchPackRequest.MerchPackType;
        Status = merchPackRequest.Status;
        RequestType = merchPackRequest.RequestType;
    }

    public long Id { get; private set; }

    /// <summary>
    /// Employee
    /// </summary>
    public Employee Employee { get; private set; }
    
    /// <summary>
    /// Employee id
    /// </summary>
    public long EmployeeId { get; set; }

    public EmployeeEventType? EventType { get; private set; }

    /// <summary>
    /// Employee email
    /// </summary>
    public string? EmployeeFullName { get; private set; }
    /// <summary>
    /// Employee email
    /// </summary>
    public string? EmployeeEmail { get; private set; }
    /// <summary>
    /// Employee clothing size
    /// </summary>
    public ClothingSize EmployeeClothingSize { get; private set; }
    /// <summary>
    /// Hr email
    /// </summary>
    public string? HrEmail { get; private set; }
    /// <summary>
    /// Merch pack type id
    /// </summary>
    public MerchType MerchPackType { get; private set; }
    
    public Status Status { get; private set; }

    public RequestType RequestType { get; private set; }
}