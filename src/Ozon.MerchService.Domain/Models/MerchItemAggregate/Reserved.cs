namespace Ozon.MerchService.Domain.Models.MerchItemAggregate;

public class Reserved : ValueObject
{
    public bool Value { get; private set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}