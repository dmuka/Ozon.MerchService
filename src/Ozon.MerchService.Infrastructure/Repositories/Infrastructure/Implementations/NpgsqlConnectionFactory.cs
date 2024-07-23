using System.Data;
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
        if (_connection != null)
        {
            return _connection;
        }

        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync(token);
        _connection.StateChange += (o, e) =>
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                _connection = null;
            }
        };
        
        return _connection;
    }

    public async void Dispose()
    {
        _connection?.Dispose();
    }
}