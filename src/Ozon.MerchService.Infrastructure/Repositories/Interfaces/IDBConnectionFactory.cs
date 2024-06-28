namespace Ozon.MerchService.Infrastructure.Repositories.Interfaces;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> Create(CancellationToken token);
}