using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchItemAggregate;

public class MerchItem : Item, IAggregationRoot
{
    public MerchItem(long id, long sku, ItemType type, string name)
    {
        Id = id;
        StockKeepingUnit = sku;
        Type = type;
        Name = name;
    }
    
    public ItemType Type { get; private set; }
    
    public long StockKeepingUnit { get; set; }

    public RequestedAt RequestedAt { get; } = new();

    public Reserved Reserved { get; } = null;
}