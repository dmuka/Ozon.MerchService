using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;

public class NpgsqlConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly string _connectionString;
    private NpgsqlConnection _connection;
    
    public NpgsqlConnectionFactory(IOptions<DbConnectionOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }
    
    public async Task<NpgsqlConnection> Create(CancellationToken token)
    {
        _connection ??= new NpgsqlConnection(_connectionString);
        
        await _connection.OpenAsync(token);

        return _connection;
    }

    public async void Dispose()
    {
        await _connection.DisposeAsync();
    }
}