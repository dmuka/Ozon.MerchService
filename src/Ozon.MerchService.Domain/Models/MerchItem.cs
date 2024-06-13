namespace Ozon.MerchService.Domain.Models;

public class MerchItem<T> : Item<T>
{
    public long StockKeepingUnit { get; set; }
}