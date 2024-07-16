using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory)
    : Repository<MerchPack, long>(connectionFactory), IMerchPacksRepository
{
    public async Task<MerchPack> GetMerchPackByMerchType(MerchType merchType, CancellationToken cancellationToken)
    {
        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE merch_type={merchType}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            var merchPack = await connection.QuerySingleAsync<MerchPack>(query);

            return merchPack;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
    }
}