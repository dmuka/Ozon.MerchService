using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.Domain.Events.Integration;

public class MerchPackIsReadyIntegrationEvent(Email employeeEmail, MerchType merchPackType) 
    : IntegrationEvent
{
    public Email EmployeeEmail { get; private set; } = employeeEmail;
    public MerchType MerchPackType { get; private set; } = merchPackType;
}