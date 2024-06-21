namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class RequestedAt : ValueObject
{
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}