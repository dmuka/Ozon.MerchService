using Ozon.MerchService.Domain.DataContracts.Attributes;

namespace Ozon.MerchService.Domain.Models;

public class Item : Entity
{
    public string Name { get; set; }
    [ColumnExclude]
    public string Description { get; set; }
}