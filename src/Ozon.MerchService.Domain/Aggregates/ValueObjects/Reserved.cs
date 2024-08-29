using Ozon.MerchService.Domain.Aggregates;

namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class Reserved : ValueObject
{
    public Reserved()
    {
    }
    
    public Reserved(bool reserved)
    {
        Value = reserved;
    }

    public bool Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}