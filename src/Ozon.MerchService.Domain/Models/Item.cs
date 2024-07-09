namespace Ozon.MerchService.Domain.Models;

public class Item : Entity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
}