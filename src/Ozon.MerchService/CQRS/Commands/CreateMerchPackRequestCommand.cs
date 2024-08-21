using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Create merch pack request command
/// </summary>
/// <param name="employeeFirstName">Employee first name</param>
/// <param name="employeeLastName">Employee last name</param>
/// <param name="employeeEmail">Employee email</param>
/// <param name="hrEmail">Hr email</param>
/// <param name="hrName">Hr name</param>
/// <param name="merchType">Merch pack type</param>
/// <param name="clothingSize">Employee clothing size</param>
/// <param name="requestType">Merch pack request type (manual, auto)</param>
public class CreateMerchPackRequestCommand(
    string employeeFirstName,
    string employeeLastName,
    string employeeEmail,
    string hrEmail,
    string hrName,
    MerchType merchType,
    ClothingSize clothingSize,
    RequestType requestType) : IRequest<RequestStatus>
{
    /// <summary>
    /// Employee email property
    /// </summary>
    public string EmployeeEmail { get; private set; } = employeeEmail;
    /// <summary>
    /// Hr email property
    /// </summary>
    public string HrEmail { get; private set; } = hrEmail;
    /// <summary>
    /// Employee name property
    /// </summary>
    public string EmployeeName { get; private set; } = employeeFirstName + employeeLastName;
    /// <summary>
    /// Hr name property
    /// </summary>
    public string HrName { get; private set; } = hrName;
    /// <summary>
    /// Employee event type property
    /// </summary>
    public EmployeeEventType? EventType { get; private set; } = null;
    /// <summary>
    /// Merch pack type property
    /// </summary>
    public MerchType MerchType { get; private set; } = merchType;
    /// <summary>
    /// Clothing size property
    /// </summary>
    public ClothingSize ClothingSize { get; private set; } = clothingSize;
    /// <summary>
    /// Merch pack request type property
    /// </summary>
    public RequestType RequestType { get; private set; } = requestType;
}