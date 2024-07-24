namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class Sku(long sku) : ValueObject
{
    public long Value { get; } = sku;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}