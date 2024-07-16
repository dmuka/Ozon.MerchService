using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPackRequestsRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory)
    : Repository<MerchPackRequest, long>(connectionFactory), IMerchPackRequestRepository
{
    public async Task<IEnumerable<MerchPackRequest>> GetByRequestStatusAsync(Status status, CancellationToken cancellationToken)
    {
        IEnumerable<MerchPackRequest> entities;

        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE status={status}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            entities = await connection.QueryAsync<MerchPackRequest>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return entities;
    }
    
    public async Task<IEnumerable<MerchPack>> GetByMerchPacksByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken)
    {
        IEnumerable<MerchPack> merchPacks;

        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE employee_id={employeeId}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            var requests = await connection.QueryAsync<MerchPackRequest>(query);

            merchPacks = requests.Select(request =>
                new MerchPack(request.MerchPackType, request.MerchItems, request.Employee.ClothingSize));
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return merchPacks;
    }
}