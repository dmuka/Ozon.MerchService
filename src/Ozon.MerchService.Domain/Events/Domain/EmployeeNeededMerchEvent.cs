using System.Text.Json;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Domain.Events.Domain;

public class EmployeeNeededMerchEvent(NotificationEvent notificationEvent) : INotification
{
    public string EmployeeEmail { get; private set; } = notificationEvent.EmployeeEmail;
    public string HrEmail { get; private set; } = notificationEvent.ManagerEmail;
    public string EmployeeName { get; private set; } = notificationEvent.EmployeeName;
    public string HrName { get; private set; } = notificationEvent.ManagerName;
    public EmployeeEventType EventType { get; private set; } = notificationEvent.EventType;
    public MerchType MerchType { get; private set; } = (MerchType)((JsonElement)notificationEvent.Payload).GetProperty(nameof(MerchType)).GetInt32();
    public ClothingSize ClothingSize { get; private set; } = (ClothingSize)((JsonElement)notificationEvent.Payload).GetProperty(nameof(ClothingSize)).GetInt32();
    public RequestType RequestType { get; private set; } = RequestType.Auto;
}