using MediatR;

namespace Ozon.MerchService.Domain.Events;

public class IntegrationEvent : INotification
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
}