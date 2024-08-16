namespace Ozon.MerchService.Domain.Models.ValueObjects;

public class Issued : ValueObject
{
    public Issued() {}
    
    public Issued(DateTimeOffset date)
    {
        Value = date;
    }
    public DateTimeOffset? Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}