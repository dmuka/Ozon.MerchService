using System.Text.Json;
using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class EmployeesRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory, IDapperQuery query) 
    : Repository(connectionFactory), IEmployeeRepository
{
    private const int Timeout = 5;

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                             SELECT
                               employees.id,
                               employees.full_name,
                               employees.email,
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
                               clothing_sizes.Id,
                               clothing_sizes.Name,
                               request_statuses.id,
                               request_statuses.name,
                               request_types.id,
                               request_types.name
                             FROM employees
                             LEFT JOIN merchpack_requests ON merchpack_requests.employee_id = employees.id
                             LEFT JOIN clothing_sizes ON clothing_sizes.id = merchpack_requests.clothing_size_id
                             LEFT JOIN merchpacks ON merchpacks.id = merchpack_requests.merchpack_type_id
                             LEFT JOIN request_statuses ON request_statuses.id = merchpack_requests.request_status_id
                             LEFT JOIN request_types ON request_types.id = merchpack_requests.request_type_id
                             WHERE email=@Email
                             """;
        
        var parameters = new
        {
            Email = email
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
                var employeeDtos = await connection
                    .QueryAsync<EmployeeDto, MerchPackRequestDto, ClothingSizeDto, RequestStatusDto, RequestTypeDto, Employee>(
                        commandDefinition,
                        (employeeDto, merchPackRequestDto, clothingSizeDto, requestStatusDto, requestTypeDto) =>
                        {
                            var merchItemDtos =
                                (JsonSerializer.Deserialize<MerchItemDto[]>(merchPackRequestDto.MerchPackItems) ?? []);

                            var merchItems = merchItemDtos.Select(dto =>
                                new MerchItem(dto.Sku, new ItemType(dto.ItemTypeId, dto.ItemTypeName), dto.Quantity)).ToArray();
                            
                            var merchPackRequest = MerchPackRequest.CreateInstance(
                                merchPackRequestDto.Id,
                                GetMerchPackType(merchPackRequestDto.MerchpackTypeId),
                                merchItems,
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
                                requestStatusDto.Name);

                            var employee = Employee.CreateInstance(
                                employeeDto.Id,
                                employeeDto.FullName,
                                string.Empty,
                                employeeDto.Email);
                            
                           employee.AddMerchPackRequest(merchPackRequest);

                           return employee;
                        });

                var result = employeeDtos
                    .GroupBy(employeeDto => employeeDto.Id)
                    .Select(group =>
                    {
                        var employee = group.First();
                
                        var merchPacksRequests = group
                             .Select(employeeDto => employeeDto.MerchPacksRequests.Single()).ToList();
                        
                        employee.SetMerchPackRequests(merchPacksRequests);
                        
                        return employee;
                    }).FirstOrDefault();

                return result;
            });
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException(ex.Message, ex);
        }
    }
}