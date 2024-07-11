using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace Ozon.MerchService.Domain.Events.Domain;

public class EmployeeNeededMerchEvent(NotificationEvent notificationEvent) : INotification
{
    public string EmployeeEmail { get; private set; } = notificationEvent.EmployeeEmail;
    public string HrEmail { get; private set; } = notificationEvent.ManagerEmail;
    public string EmployeeName { get; private set; } = notificationEvent.EmployeeName;
    public string HrName { get; private set; } = notificationEvent.ManagerName;
    public EmployeeEventType EventType { get; private set; } = notificationEvent.EventType;
    public MerchType MerchType { get; private set; } = ((MerchDeliveryEventPayload)notificationEvent.Payload).MerchType;
    public ClothingSize ClothingSize { get; private set; } = ((MerchDeliveryEventPayload)notificationEvent.Payload).ClothingSize;
}