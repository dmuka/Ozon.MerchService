namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> Create(CancellationToken token);
}