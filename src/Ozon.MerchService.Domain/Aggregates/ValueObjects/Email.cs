using Ozon.MerchService.Domain.Constants;
using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;

public class Email : ValueObject, IRegexValid
{
    public Email(string email)
    {
        if (!((IRegexValid)this).IsValid(email, EmailConstants.RegexPattern))
        {
            throw new ArgumentException($"Incorrect email value: {email}", nameof(email));
        }

        Value = email;
    }
    
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}