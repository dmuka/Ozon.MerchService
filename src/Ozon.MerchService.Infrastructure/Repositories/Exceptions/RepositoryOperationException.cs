namespace Ozon.MerchService.Infrastructure.Repositories.Exceptions;

public class RepositoryOperationException : Exception
{
    public RepositoryOperationException()
    {
    }

    public RepositoryOperationException(string description) : base(description)
    {
    }

    public RepositoryOperationException(string description, Exception innerException) : base(description, innerException)
    {
    }
}