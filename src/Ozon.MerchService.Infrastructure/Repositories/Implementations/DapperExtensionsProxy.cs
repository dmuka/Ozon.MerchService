using Dapper;
using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

internal sealed class DapperExtensionsProxy(IUnitOfWork unitOfWork)
{
    internal async ValueTask<T?> ExecuteScalarAsync<T>(string sqlQuery)
    {
        var result = await unitOfWork.Connection.ExecuteScalarAsync<T>(sqlQuery);

        return result;
    }

    internal async ValueTask<T?> ExecuteWithParamScalarAsync<T>(string sqlQuery, Dictionary<string, object> parameters)
    {
        var dynamicParameters = new DynamicParameters(parameters);
        
        var result = await unitOfWork.Connection.ExecuteScalarAsync<T>(sqlQuery, dynamicParameters);

        return result;
    }

    internal async ValueTask<int> ExecuteAsync(string sqlQuery)
    {
        var result = await unitOfWork.Connection.ExecuteAsync(sqlQuery);

        return result;
    }

    internal async ValueTask<int> ExecuteWithParamAsync<T>(string sqlQuery, Dictionary<string, object> parameters)
    {
        var dynamicParameters = new DynamicParameters(parameters);
        
        var result = await unitOfWork.Connection.ExecuteAsync(sqlQuery, dynamicParameters);

        return result;
    }

    internal async Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);
        
        var result = await unitOfWork.Connection.QueryAsync<T>(sqlQuery);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    internal async Task<IEnumerable<T>> QueryWithParamAsync<T>(
        string sqlQuery, 
        Dictionary<string, object> parameters, 
        CancellationToken cancellationToken)
    {
        var dynamicParameters = new DynamicParameters(parameters);

        await unitOfWork.StartTransaction(cancellationToken);
        
        var result = await unitOfWork.Connection.QueryAsync<T>(sqlQuery, dynamicParameters);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    internal IEnumerable<T> Query<T>(string sqlQuery)
    {
        var result = unitOfWork.Connection.Query<T>(sqlQuery);

        return result;
    }

    internal IEnumerable<T> QueryWithParam<T>(string sqlQuery, Dictionary<string, object> parameters)
    {
        var dynamicParameters = new DynamicParameters(parameters);
        
        var result = unitOfWork.Connection.Query<T>(sqlQuery, dynamicParameters);

        return result;
    }
}