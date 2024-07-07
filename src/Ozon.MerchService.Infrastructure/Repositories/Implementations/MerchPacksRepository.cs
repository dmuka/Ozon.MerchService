using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(IUnitOfWork unitOfWork) : BaseRepository<MerchPack>, IMerchPacksRepository
{    
    public async Task<long> CreateAsync(MerchPack merchPack, CancellationToken cancellationToken)
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
    
    public async Task<MerchPack?> GetByIdAsync(long merchPackId, CancellationToken cancellationToken)
    {
        MerchPack? merchPack;
        
        var tableName = GetTableName();
        var keyColumnName = GetKeyColumnName();
            
        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE {keyColumnName}={merchPackId}";
        
        try
        {
            await unitOfWork.StartTransaction(cancellationToken);

            merchPack = await unitOfWork.Connection.QuerySingleOrDefaultAsync<MerchPack>(query);
            
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

        return merchPack;
    }
    
    public async Task<IEnumerable<MerchPack>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<MerchPack> merchPacks;
        
        var tableName = GetTableName();
            
        var query = $"SELECT {GetColumnsNames()} FROM {tableName}";
        
        try
        {
            await unitOfWork.StartTransaction(cancellationToken);

            merchPacks = await unitOfWork.Connection.QueryAsync<MerchPack>(query);
            
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

        return merchPacks;
    }
    
    public async Task<long> UpdateAsync(MerchPack merchPack, CancellationToken cancellationToken)
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

                var columnValue = properties[i].GetValue(merchPack);
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
    
    public async Task<long> DeleteAsync(long merchPackId, CancellationToken cancellationToken)
    {
        int rowsAffected;
        
        var tableName = GetTableName();
        var keyColumnName = GetKeyColumnName();
            
        var query = $"DELETE FROM {tableName} WHERE {keyColumnName}={merchPackId}";
        
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