using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.DataContracts;
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
public class Repository(IDbConnectionFactory<NpgsqlConnection> connectionFactory) 
    : BaseRepository, IRepository
{
    public async Task<TId> CreateAsync<T, TId>(CancellationToken cancellationToken,  object parameters) 
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
            throw new RepositoryOperationException();
        }
        
        return entityId;
    }

    public async Task<T?> GetByIdAsync<T, TId>(TId entityId, CancellationToken cancellationToken)
        where TId : IEquatable<TId>
    {
        T? entity;

        var tableName = GetTableName<T>();
        var keyColumnName = GetKeyColumnName<T>();

        var query = $"SELECT {GetColumnsNames<T>()} FROM {tableName} WHERE {keyColumnName}={entityId}";

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

    public async Task<IEnumerable<T>> GetAllAsync<T>(CancellationToken cancellationToken)
    {
        IEnumerable<T> entities;

        var tableName = GetTableName<T>();

        var query = $"SELECT {GetColumnsNames<T>()} FROM {tableName}";

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

    public async Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken)
    {
        int rowsAffected;

        try
        {
            var tableName = GetTableName<T>();
            var keyColumnName = GetKeyColumnName<T>();
            var keyColumnValue = GetKeyPropertyValue<T>();

            var query = new StringBuilder(50);

            query.Append($"UPDATE {tableName} SET ");

            var properties = GetProperties<T>(true).ToArray();

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