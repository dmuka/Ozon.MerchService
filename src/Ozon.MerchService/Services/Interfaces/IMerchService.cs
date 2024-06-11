using Ozon.MerchandizeService.Models;

namespace Ozon.MerchandizeService.Services.Interfaces;

/// <summary>
/// Interface for merch service
/// </summary>
public interface IMerchService
{
    /// <summary>
    /// Get all stock items
    /// </summary>
    /// <returns>List of all stock items</returns>
    public List<StockItem> GetAll();
    
    /// <summary>
    /// Get stock item by id
    /// </summary>
    /// <param name="itemId">Stock item id</param>
    /// <returns>Stock item or null if not found</returns>
    public StockItem? GetById(long itemId);
}