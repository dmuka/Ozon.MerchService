using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.Models;

public class Item : Entity<long>
{
    public string Name { get; set; }
}