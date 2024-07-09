using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace Ozon.MerchService.Domain.Events;

public class EmployeeEvent(NotificationEvent notificationEvent) : INotification
{
    public NotificationEvent NotificationEvent { get; } = notificationEvent;
}