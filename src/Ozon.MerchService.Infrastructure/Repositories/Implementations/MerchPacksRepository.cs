using System.Text.Json;
using AutoMapper;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(
    IDbConnectionFactory<NpgsqlConnection> connectionFactory, 
    IDapperQuery query,
    IMapper mapper)
    : Repository(connectionFactory, query, mapper), IMerchPacksRepository
{
    private readonly IDapperQuery _query = query;
    private const int Timeout = 5;
    
    public async Task<MerchPack?> GetMerchPackById(int merchPackId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                SELECT
                                  merchpacks.id,
                                  merchpacks.name,
                                  merchpacks.items
                                FROM merchpacks
                                WHERE id=@MerchPackId
                                """;
        
        var parameters = new
        {
            MerchPackId = merchPackId
        };        
            
        var commandDefinition = new CommandDefinition(
            sqlQuery,
            parameters: parameters,
            commandTimeout: Timeout,
            cancellationToken: cancellationToken);

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            return await _query.Call(async () =>
            {
                var merchPackDto = await connection
                    .QuerySingleOrDefaultAsync<MerchPackDto>(commandDefinition);
                
                if (merchPackDto is null) return null;

                var items = JsonSerializer.Deserialize<MerchItemDto[]>(merchPackDto.Items) ?? [];
                
                var merchPackItems = items.Select(item =>
                    new MerchItem(default, new ItemType(item.ItemTypeId, item.ItemTypeName), item.Quantity));

                var result = new MerchPack(GetMerchPackType(merchPackId), merchPackItems);

                return result;
            });
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException(ex.Message, ex);
        }
    }
}