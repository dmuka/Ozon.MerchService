using Ozon.MerchService.Domain.Constants;
using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;

public class FullName : ValueObject, IRegexValid
{
    public FullName(string firstName, string lastName)
    {
        if (!((IRegexValid)this).IsValid(firstName, NameConstants.PartRegexPattern))
        {
            throw new ArgumentException($"{nameof(FullName)}: Incorrect first name value: {firstName}", nameof(firstName));
        }
        
        if (!((IRegexValid)this).IsValid(lastName, NameConstants.PartRegexPattern))
        {
            throw new ArgumentException($"{nameof(FullName)}: Incorrect last name value: {lastName}", nameof(lastName));
        }

        Value = firstName + " " + lastName;
    }
    
    public FullName(string fullName)
    {
        if (!((IRegexValid)this).IsValid(fullName, NameConstants.FullNameRegexPattern))
        {
            throw new ArgumentException($"Incorrect fullName value: {fullName}", nameof(fullName));
        }

        Value = fullName;
    }
    
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}