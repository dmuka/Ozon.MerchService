using Ozon.MerchandizeService.Models;
using Ozon.MerchandizeService.Services.Interfaces;

namespace Ozon.MerchandizeService.Services.Implementations;

/// <summary>
/// Merch service
/// </summary>
public class MerchService : IMerchService
{
    private readonly List<StockItem> StockItems =
    [
        new StockItem(1, "First stock item", 10),
        new StockItem(2, "Second stock item", 15),
        new StockItem(3, "Third stock item", 20)
    ];

    /// <summary>
    /// Get all stock items
    /// </summary>
    public List<StockItem> GetAll() => StockItems;

    /// <summary>
    /// Get stock item by id
    /// </summary>
    /// <param name="itemId">Item id</param>
    /// <returns>Stock item or null if not found</returns>
    public StockItem? GetById(long itemId)
    {
        return StockItems.Find(item => item.ItemId == itemId);
    }
}