namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class MerchItemDto
{
    public int ItemTypeId { get; set; }
    public string ItemTypeName { get; set; }
    public long Sku { get; set; }
    
    public int Quantity { get; set; }
}