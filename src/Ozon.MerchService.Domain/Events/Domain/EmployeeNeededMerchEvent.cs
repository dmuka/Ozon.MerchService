using System.Text.Json;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Domain.Events.Domain;

public class EmployeeNeededMerchEvent : INotification
{
    public EmployeeNeededMerchEvent(NotificationEvent notificationEvent)
    {
        EmployeeEmail = notificationEvent.EmployeeEmail;
        HrEmail = notificationEvent.ManagerEmail;
        EmployeeName = notificationEvent.EmployeeName;
        HrName = notificationEvent.ManagerName;
        EventType = notificationEvent.EventType;
        MerchType = (MerchType)((JsonElement)notificationEvent.Payload).GetProperty(nameof(MerchType)).GetInt32();
        ClothingSize = (ClothingSize)((JsonElement)notificationEvent.Payload).GetProperty(nameof(ClothingSize)).GetInt32();
        RequestType = RequestType.Auto;
    }
    
    public string EmployeeEmail { get; private set; }
    public string HrEmail { get; private set; }
    public string EmployeeName { get; private set; }
    public string HrName { get; private set; }
    public EmployeeEventType EventType { get; private set; }
    public MerchType MerchType { get; private set; }
    public ClothingSize ClothingSize { get; private set; }
    public RequestType RequestType { get; private set; }
}