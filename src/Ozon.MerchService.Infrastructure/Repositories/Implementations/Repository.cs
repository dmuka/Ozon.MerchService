using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class Repository<T, TId>(IUnitOfWork unitOfWork) : BaseRepository<T>
where T : IAggregationRoot
where TId : IEquatable<TId>
{
    public async Task<long> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        int rowsAffected;

        try
        {
            var tableName = GetTableName();
            var columns = GetColumns(excludeKey: true);
            var properties = GetPropertyValues(excludeKey: true);

            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

            await unitOfWork.StartTransaction(cancellationToken);

            rowsAffected = await unitOfWork.Connection.ExecuteAsync(query);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        finally
        {
            await unitOfWork.Connection.CloseAsync();
        }

        return rowsAffected;
    }

    public async Task<T?> GetByIdAsync(TId entityId, CancellationToken cancellationToken)
    {
        T? entity;

        var tableName = GetTableName();
        var keyColumnName = GetKeyColumnName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE {keyColumnName}={entityId}";

        try
        {
            await unitOfWork.StartTransaction(cancellationToken);

            entity = await unitOfWork.Connection.QuerySingleOrDefaultAsync<T>(query);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        finally
        {
            await unitOfWork.Connection.CloseAsync();
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
            await unitOfWork.StartTransaction(cancellationToken);

            entities = await unitOfWork.Connection.QueryAsync<T>(query);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        finally
        {
            await unitOfWork.Connection.CloseAsync();
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

            rowsAffected = await unitOfWork.Connection.ExecuteAsync(query.ToString());
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        finally
        {
            await unitOfWork.Connection.CloseAsync();
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
            await unitOfWork.StartTransaction(cancellationToken);

            rowsAffected = await unitOfWork.Connection.ExecuteAsync(query);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }
        finally
        {
            await unitOfWork.Connection.CloseAsync();
        }

        return rowsAffected;
    }
}