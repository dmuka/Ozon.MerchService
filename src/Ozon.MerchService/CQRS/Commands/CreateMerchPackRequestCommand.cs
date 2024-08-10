using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Commands;

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
    public string EmployeeEmail { get; private set; } = employeeEmail;
    public string HrEmail { get; private set; } = hrEmail;
    public string EmployeeName { get; private set; } = employeeFirstName + employeeLastName;
    public string HrName { get; private set; } = hrName;
    public EmployeeEventType? EventType { get; private set; } = null;
    public MerchType MerchType { get; private set; } = merchType;
    public ClothingSize ClothingSize { get; private set; } = clothingSize;
    public RequestType RequestType { get; private set; } = requestType;
}