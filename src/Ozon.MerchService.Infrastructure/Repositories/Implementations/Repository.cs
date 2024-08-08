using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using AutoMapper;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

/// <summary>
/// Generic repository for basic operations with entities
/// </summary>
/// <param name="connectionFactory">Connection factory</param>
public class Repository(
    IDbConnectionFactory<NpgsqlConnection> connectionFactory,
    IDapperQuery dapperQuery,
    IMapper mapper) 
    : BaseRepository, IRepository
{
    public async Task<TId> CreateAsync<T, TId>(CancellationToken cancellationToken, object parameters) 
        where TId : IEquatable<TId>
    {
        TId entityId;
        
        try
        {
            var tableName = GetTableName<T>();
            var keyColumnName = GetKeyColumnName<T>();
            var columns = GetColumns<T>(excludeKey: true);
            var properties = GetPropertyValues<T>(excludeKey: true);

            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties}) RETURNING {keyColumnName}";

            var connection = await GetConnection(cancellationToken);

            entityId = await connection.QuerySingleAsync<TId>(query, parameters);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException(ex.Message, ex);
        }
        
        return entityId;
    }

    public async Task<T?> GetByIdAsync<T, TId>(TId entityId, CancellationToken cancellationToken)
        where TId : IEquatable<TId>
    {
        var tableName = GetTableName<T>();
        var keyColumnName = GetKeyColumnName<T>();

        var query = $"SELECT {GetColumnsNames<T>()} FROM {tableName} WHERE {keyColumnName}={entityId}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            var entity = await connection.QuerySingleOrDefaultAsync<T>(query);

            return entity;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(CancellationToken cancellationToken)
    {
        var tableName = GetTableName<T>();

        var query = $"SELECT {GetColumnsNames<T>()} FROM {tableName}";

        try
        {
            var connection = await GetConnection(cancellationToken);

            var entities = await connection.QueryAsync<T>(query);

            return entities;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
    }

    public async Task<int> UpdateAsync<T, TDto>(T entity, CancellationToken cancellationToken)
        where T : Entity
        where TDto : BaseEntity
    {
        try
        {
            var tableName = GetTableName<TDto>();
            var keyColumnName = GetKeyColumnName<TDto>();
            var keyColumnValue = GetKeyPropertyValue<TDto>();

            var query = new StringBuilder(50);

            query.Append($"UPDATE {tableName} SET ");

            var properties = GetProperties<TDto>(true).ToArray();

            var dto = mapper.Map<TDto>(entity);

            var parameters = new { dto.Id };
            
            for (var i = 0; i < properties.Length; i++)
            {
                var columnAttribute = properties[i].GetCustomAttribute<ColumnAttribute>();

                object? columnValue = null;
                
                if (properties[i].PropertyType == typeof(string))
                {
                    columnValue = "'" + properties[i].GetValue(dto) + "'";
                }
                else if (properties[i].PropertyType == typeof(DateTimeOffset))
                {
                    columnValue = $"CAST('{properties[i].GetValue(dto)}' AS timestamptz)";
                }
                else if (properties[i].PropertyType == typeof(DateTimeOffset?))
                {
                    var value = properties[i].GetValue(dto);
                    
                    columnValue = value is null ? "null" : $"CAST('{value}' AS timestamptz)";
                }
                else
                {
                    columnValue = properties[i].GetValue(dto);
                }
                
                var columnName = columnAttribute?.Name;
                
                query.Append($"{columnName}={columnValue},");
            }

            query.Remove(query.Length - 1, 1);

            query.Append($" WHERE {keyColumnName} = @{keyColumnValue}");
            
            var connection = await GetConnection(cancellationToken);

            var rowsAffected = await dapperQuery.CallAction(entity, async () =>
            {
                var rows = await connection.ExecuteAsync(query.ToString(), parameters);

                return rows;
            });
            
            return rowsAffected;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException(ex.Message, ex);
        }
    }

    public async Task<int> DeleteAsync<T, TId>(TId entityId, CancellationToken cancellationToken)
        where TId : IEquatable<TId>
    {
        int rowsAffected;

        var tableName = GetTableName<T>();
        var keyColumnName = GetKeyColumnName<T>();

        var query = $"DELETE FROM {tableName} WHERE {keyColumnName}={entityId}";

        try
        {
            var connection = await GetConnection(cancellationToken);

            rowsAffected = await connection.ExecuteAsync(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return rowsAffected;
    }

    internal async Task<NpgsqlConnection> GetConnection(CancellationToken cancellationToken)
    {
        return await connectionFactory.Create(cancellationToken);
    }
}