using Ozon.MerchService.Domain.Aggregates;

namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class RequestedAt : ValueObject
{
    public RequestedAt() {}
    
    public RequestedAt(DateTimeOffset date)
    {
        Value = date;
    }
    
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}