using System.Text.RegularExpressions;

namespace Ozon.MerchService.Domain.Models;

public interface IRegexValid
{
    public bool IsValid(string value, string pattern)
    {
        var regex = new Regex(pattern);  
        var match = regex.Match(value);
        
        return match.Success;
    }
}