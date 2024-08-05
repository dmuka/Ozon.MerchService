namespace Ozon.MerchService.Infrastructure.Repositories.Exceptions;

public class BrokerException : Exception
{
    public BrokerException()
    {
    }

    public BrokerException(string description) : base(description)
    {
    }

    public BrokerException(string description, Exception innerException) : base(description, innerException)
    {
    }
}