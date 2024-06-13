namespace Ozon.MerchService.Domain.Models;

public class Item<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
}