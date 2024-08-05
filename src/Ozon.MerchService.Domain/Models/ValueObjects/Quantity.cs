namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class Quantity(int quantity) : ValueObject
{
    public int Value { get; } = quantity;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}