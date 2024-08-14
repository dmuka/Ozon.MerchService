using Ozon.MerchService.Domain.Constants;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class HrEmail : ValueObject, IRegexValid
{
    public HrEmail(string email)
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