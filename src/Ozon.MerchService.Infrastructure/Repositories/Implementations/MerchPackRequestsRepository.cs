using System.Text.Json;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPackRequestsRepository(
    IDbConnectionFactory<NpgsqlConnection> connectionFactory, 
    IDapperQuery query,
    IMapper mapper)
    : Repository(connectionFactory, query, mapper), IMerchPackRequestRepository
{
    private const int Timeout = 5;
    
    public async Task<IEnumerable<MerchPackRequest>> GetByRequestStatusAsync(RequestStatus requestStatus, CancellationToken cancellationToken)
    {
        IEnumerable<MerchPackRequest> entities;

        var tableName = GetTableName<MerchPackRequest>();

        var query = $"SELECT {GetColumnsNames<MerchPackRequest>()} FROM {tableName} WHERE status={requestStatus}";

        try
        {
            var connection = await GetConnection(cancellationToken);
            
            entities = await connection.QueryAsync<MerchPackRequest>(query);
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException();
        }

        return entities;
    }
    
    public async Task<IEnumerable<MerchPackRequest>> GetAllByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                             SELECT
                               merchpack_requests.id,
                               merchpack_requests.merchpack_type_id,
                               merchpack_requests.merchpack_items,
                               merchpack_requests.employee_id,
                               merchpack_requests.clothing_size_id,
                               merchpack_requests.hr_email,
                               merchpack_requests.request_type_id,
                               merchpack_requests.requested_at,
                               merchpack_requests.issued,
                               merchpack_requests.request_status_id,
                               clothing_sizes.name,
                               employees.full_name,
                               employees.email
                             FROM merchpack_requests 
                             LEFT JOIN clothing_sizes ON clothing_sizes.id = merchpack_requests.clothing_size_id
                             LEFT JOIN request_statuses ON request_statuses.id = merchpack_requests.request_status_id
                             LEFT JOIN request_types ON request_types.id = merchpack_requests.request_type_id
                             LEFT JOIN employees ON employees.Id = merchpack_requests.employee_id
                             LEFT JOIN merchpacks ON merchpacks.Id = merchpack_requests.merchpack_type_id
                             WHERE employee_id=@EmployeeId
                             """;
        
        var parameters = new
        {
            EmployeeId = employeeId
        };
        
        var commandDefinition = new CommandDefinition(
            sqlQuery,
            parameters: parameters,
            commandTimeout: Timeout,
            cancellationToken: cancellationToken);

        try
        {
            var connection = await GetConnection(cancellationToken);

            return await query.Call(async () =>
            {
                var merchPackRequests = await connection.QueryAsync<
                    MerchPackRequestDto, EmployeeDto, ClothingSizeDto, MerchPackDto, RequestStatusDto, RequestTypeDto, MerchPackRequest>(
                    commandDefinition,
                    (merchPackRequestDto, employeeDto, clothingSizeDto, merchPackDto, requestTypeDto, requestStatusDto) =>
                        MerchPackRequest.CreateInstance(
                            merchPackRequestDto.Id,
                            GetMerchPackType(merchPackRequestDto.MerchpackTypeId),
                            JsonSerializer.Deserialize<MerchItem[]>(merchPackRequestDto.MerchPackItems),
                            merchPackRequestDto.EmployeeId,
                            employeeDto.FullName,
                            employeeDto.Email,
                            GetClothingSize(clothingSizeDto.Id),
                            merchPackRequestDto.HrEmail,
                            requestTypeDto.Id,
                            requestTypeDto.Name,
                            merchPackRequestDto.RequestedAt,
                            merchPackRequestDto.Issued,
                            requestStatusDto.Id,
                            requestStatusDto.Name));
                
                return merchPackRequests;
            });
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException(ex.Message, ex);
        }
    }
}