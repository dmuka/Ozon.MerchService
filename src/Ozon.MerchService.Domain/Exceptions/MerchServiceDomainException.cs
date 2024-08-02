namespace Ozon.MerchService.Domain.Exceptions;

public class MerchServiceDomainException : BaseDomainException
{
    public MerchServiceDomainException()
    {
    }

    public MerchServiceDomainException(string description) : base(description)
    {
    }

    public MerchServiceDomainException(string description, Exception innerException) : base(description, innerException)
    {
    }
}