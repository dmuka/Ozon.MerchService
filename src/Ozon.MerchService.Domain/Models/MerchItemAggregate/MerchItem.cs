using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchItemAggregate;

public class MerchItem : Item
{
    public long StockKeepingUnit { get; set; }

    public RequestedAt RequestedAt { get; } = new();

    public Reserved Reserved { get; } = new();
}