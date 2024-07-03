using Ozon.MerchService.Domain.Constants;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class FullName : ValueObject, IRegexValid
{
    public FullName(string firstName, string lastName)
    {
        if (!((IRegexValid)this).IsValid(firstName, NameConstants.RegexPattern))
        {
            throw new ArgumentException($"Incorrect first name value: {firstName}", nameof(firstName));
        }
        
        if (!((IRegexValid)this).IsValid(firstName, NameConstants.RegexPattern))
        {
            throw new ArgumentException($"Incorrect last name value: {lastName}", nameof(lastName));
        }

        Value = firstName + " " + lastName;
    }
    
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}