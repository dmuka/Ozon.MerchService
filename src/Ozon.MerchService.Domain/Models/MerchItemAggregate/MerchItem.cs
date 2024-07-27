using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchItemAggregate;

public class MerchItem(long sku, ItemType type) : IAggregationRoot
{
    public ItemType Type { get; private set; } = type;
    public Sku Sku { get; private set; } = new(sku);

    public void SetSku(long sku)
    {
        Sku = new Sku(sku);
    }
}