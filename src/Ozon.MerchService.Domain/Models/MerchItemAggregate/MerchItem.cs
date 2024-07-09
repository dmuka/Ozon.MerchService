using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.ValueObjects;

namespace Ozon.MerchService.Domain.Models.MerchItemAggregate;

public class MerchItem : Item, IAggregationRoot
{
    public MerchItem(long id, long sku, string name)
    {
        Id = id;
        StockKeepingUnit = sku;
        Name = name;
    }
    
    public long StockKeepingUnit { get; set; }

    public RequestedAt RequestedAt { get; } = new();

    public Reserved Reserved { get; } = null;
}