namespace Ozon.MerchandizeService.Models;

public class StockItem(long itemId, string itemName, int quantity)
{
    public long ItemId { get; }
    public string ItemName { get; }
    public int Quantity { get; }
}