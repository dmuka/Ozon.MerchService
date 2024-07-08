using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;

public class NpgsqlConnectionFactory(IOptions<DbConnectionOptions> options) : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly DbConnectionOptions _options = options.Value;
    private  NpgsqlConnection? _connection;

    public async Task<NpgsqlConnection> Create(CancellationToken token)
    {
        if (_connection is not null) return _connection;

        _connection = new NpgsqlConnection(_options.ConnectionString);
        
        await _connection.OpenAsync(token);
        
        _connection.StateChange += (_, eventArgs) =>
        {
            if (eventArgs.CurrentState == ConnectionState.Closed) _connection = null;
        };
        
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
            
        GC.SuppressFinalize(this);
    }
}