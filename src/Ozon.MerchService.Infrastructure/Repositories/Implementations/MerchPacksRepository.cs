using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory)
    : Repository<MerchPack, long>(connectionFactory), IMerchPacksRepository
{
    public async Task<MerchPack> GetMerchPackById(int merchPackId, CancellationToken cancellationToken)
    {
        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE id={merchPackId}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            var merchPackDto = await connection.QuerySingleAsync<MerchPackDto>(query);

            var merchPack = MerchPack.CreateInstance(merchPackDto.Id,
                new MerchPack(MerchType.VeteranPack, new MerchItem[] { }, ClothingSize.L));

            return merchPack;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
    }
}