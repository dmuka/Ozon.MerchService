using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class EmployeesRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory) 
    : Repository<Employee, long>(connectionFactory), IEmployeeRepository
{

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        Employee? entity;

        var tableName = GetTableName();

        var query = $"SELECT {GetColumnsNames()} FROM {tableName} WHERE email={email}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            entity = await connection.QuerySingleOrDefaultAsync<Employee>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return entity;
    }
}