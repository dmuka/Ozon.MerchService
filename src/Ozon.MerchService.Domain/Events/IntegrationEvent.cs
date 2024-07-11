namespace Ozon.MerchService.Domain.Events;

public class IntegrationEvent
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
}