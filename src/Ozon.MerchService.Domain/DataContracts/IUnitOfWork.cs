using Npgsql;

namespace Ozon.MerchService.Domain.DataContracts;

public interface IUnitOfWork
{
    ValueTask StartTransaction(CancellationToken token);
    
    NpgsqlConnection Connection { get; }
        
    Task SaveChangesAsync(CancellationToken cancellationToken);
}