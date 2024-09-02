using System.Text.Json;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler for create merch pack request
/// </summary>
/// <param name="mediator">Mediator instance</param>
/// <param name="unitOfWork">Unit of work instance</param>
/// <param name="repository">Generic repository instance</param>
/// <param name="employeeRepository">Employee repository instance</param>
public class CreateMerchPackRequestCommandHandler(
    IMediator mediator,  
    IUnitOfWork unitOfWork,
    IRepository repository,
    IEmployeeRepository employeeRepository) : IRequestHandler<CreateMerchPackRequestCommand, RequestStatus>
{
    /// <summary>
    /// Creates merch pack request
    /// </summary>
    /// <param name="command">Command with merch pack request data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<RequestStatus> Handle(CreateMerchPackRequestCommand command, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);    
        
        var employee = await employeeRepository.GetByEmailAsync(command.EmployeeEmail, cancellationToken);

        if (employee is null)
        {
            var employeeId = await employeeRepository.CreateAsync<Employee, long>(
                new { FullName = command.EmployeeName, Email = command.EmployeeEmail }, 
                cancellationToken);
            
            employee = Employee.CreateInstance(
                employeeId, 
                command.EmployeeName, 
                string.Empty, 
                command.EmployeeEmail);
        }

        var merchPackQuery = new GetMerchPackQuery(command.MerchType, command.ClothingSize);

        var merchPack = await mediator.Send(merchPackQuery, cancellationToken);
        
        var dto = GetDto(command, merchPack, employee);
            
        var merchPackRequestId = await repository.CreateAsync<MerchPackRequestDto, long>(dto, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var merchPackRequest = MerchPackRequest.CreateInstance(
            merchPackRequestId,
            employee,
            merchPack,
            new Email(command.HrEmail),
            command.ClothingSize,
            RequestType.Auto);
        
        var reserveMerchPackCommand = new ReserveMerchPackCommand(merchPackRequest, command.EventType);
            
        var result = await mediator.Send(reserveMerchPackCommand, cancellationToken);

        return result;
    }

    private MerchPackRequestDto GetDto(CreateMerchPackRequestCommand command, MerchPack merchPack, Employee employee)
    {
        var dto = new MerchPackRequestDto()
        {
            MerchpackTypeId = (int)command.MerchType,
            MerchPackItems = JsonSerializer.Serialize(merchPack.Items.Select(item => new
            {
                item.Type.ItemTypeId,
                item.Type.ItemTypeName,
                Sku = item.Sku.Value,
                Quantity = item.Quantity.Value
            })),
            EmployeeId = employee.Id,
            ClothingSizeId = (int)command.ClothingSize,
            HrEmail = command.HrEmail,
            RequestTypeId = command.RequestType.Id,
            RequestedAt = DateTimeOffset.UtcNow,
            RequestStatusId = RequestStatus.Created.Id,
            Issued = null
        };

        return dto;
    }
}