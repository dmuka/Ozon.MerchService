using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

/// <summary>
/// Generic repository for basic operations with entities
/// </summary>
/// <param name="connectionFactory">Connection factory</param>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TId">Entity id type</typeparam>
public class Repository<T, TId>(IDbConnectionFactory<NpgsqlConnection> connectionFactory) : BaseRepository<T>
where T : IAggregationRoot
where TId : IEquatable<TId>
{
    public async Task<TId> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        TId entityId;
        
        try
        {
            var tableName = GetTableName();
            var keyColumnName = GetKeyColumnName();
            var columns = GetColumns(excludeKey: true);
            var properties = GetPropertyValues(excludeKey: true);

            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties}) RETURNING {keyColumnName}";

            var connection = await GetConnection(cancellationToken);

            entityId = await connection.QuerySingleAsync<TId>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        
        return entityId;
    }

    public async Task<T?> GetByIdAsync(TId entityId, CancellationToken cancellationToken)
    {
        T? entity;

        var tableName = GetTableName();
        var keyColumnName = GetKeyColumnName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE {keyColumnName}={entityId}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            entity = await connection.QuerySingleOrDefaultAsync<T>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<T> entities;

        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName}";

        try
        {
            var connection = await GetConnection(cancellationToken);

            entities = await connection.QueryAsync<T>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return entities;
    }

    public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        int rowsAffected;

        try
        {
            var tableName = GetTableName();
            var keyColumnName = GetKeyColumnName();
            var keyColumnValue = GetKeyPropertyValue();

            var query = new StringBuilder(50);

            query.Append($"UPDATE {tableName} SET ");

            var properties = GetProperties(true).ToArray();

            for (var i = 0; i < properties.Length; i++)
            {
                var columnAttribute = properties[i].GetCustomAttribute<ColumnAttribute>();

                var columnValue = properties[i].GetValue(entity);
                var columnName = columnAttribute?.Name;

                query.Append($"{columnName}=@{columnValue},");
            }

            query.Remove(query.Length - 1, 1);

            query.Append($" WHERE {keyColumnName} = @{keyColumnValue}");
            
            var connection = await GetConnection(cancellationToken);

            rowsAffected = await connection.ExecuteAsync(query.ToString());
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return rowsAffected;
    }

    public async Task<int> DeleteAsync(TId entityId, CancellationToken cancellationToken)
    {
        int rowsAffected;

        var tableName = GetTableName();
        var keyColumnName = GetKeyColumnName();

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