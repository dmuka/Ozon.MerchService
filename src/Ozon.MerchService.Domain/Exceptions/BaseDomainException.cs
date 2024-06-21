namespace Ozon.MerchService.Domain.Exceptions;

/// <summary>
/// Base domain exception
/// </summary>
[Serializable]
public class BaseDomainException : Exception
{
    public BaseDomainException()
    {
    }

    public BaseDomainException(string description) : base(description)
    {
    }

    public BaseDomainException(string description, Exception innerException) : base(description, innerException)
    {
    }
}