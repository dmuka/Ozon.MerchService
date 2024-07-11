using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.Domain.Events.Integration;

public class MerchReplenishedEvent(Email employeeEmail, MerchType merchPackType) : IntegrationEvent
{
    public Email EmployeeEmail { get; } = employeeEmail;
    public MerchType MerchPackType { get; } = merchPackType;
}