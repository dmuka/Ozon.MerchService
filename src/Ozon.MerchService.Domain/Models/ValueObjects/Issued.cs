namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class Issued : ValueObject
{
    public DateTimeOffset? Value { get; } = null;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}