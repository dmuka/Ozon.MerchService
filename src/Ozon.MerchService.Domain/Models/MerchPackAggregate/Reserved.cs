namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class Reserved(bool reserved) : ValueObject
{
    public bool Value { get; private set; } = reserved;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}