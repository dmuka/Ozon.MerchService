using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;


namespace Ozon.MerchService.Domain.Events.Integration;

public class MerchEndedEvent(Email hrEmail, MerchType merchPackType) : IntegrationEvent
{
    public Email HrEmail { get; } = hrEmail;
    public MerchType MerchPackType { get; } = merchPackType;
}