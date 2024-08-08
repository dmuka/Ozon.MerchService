using System.Text.Json;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class EmployeeNeededMerchEventHandler(
    IMediator mediator,  
    IUnitOfWork unitOfWork,
    IRepository repository,
    IEmployeeRepository employeeRepository)
    : INotificationHandler<EmployeeNeededMerchEvent>
{
    public async Task Handle(EmployeeNeededMerchEvent employeeNeededMerchEvent, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);    
        
        var employee = await employeeRepository.GetByEmailAsync(employeeNeededMerchEvent.EmployeeEmail, cancellationToken);

        if (employee is null)
        {
            var employeeId = await employeeRepository.CreateAsync<Employee, long>(
                cancellationToken, 
                new { FullName = employeeNeededMerchEvent.EmployeeName, Email = employeeNeededMerchEvent.EmployeeEmail });
            
            employee = Employee.CreateInstance(
                employeeId, 
                employeeNeededMerchEvent.EmployeeName, 
                string.Empty, 
                employeeNeededMerchEvent.EmployeeEmail);
        }

        var merchPackQuery =
            new GetMerchPackQuery(employeeNeededMerchEvent.MerchType, employeeNeededMerchEvent.ClothingSize);

        var merchPack = await mediator.Send(merchPackQuery, cancellationToken);
        
        var dto = GetDto(employeeNeededMerchEvent, merchPack, employee);
            
        var merchPackRequestId = await repository.CreateAsync<MerchPackRequestDto, long>(cancellationToken, dto);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var merchPackRequest = MerchPackRequest.CreateInstance(
            merchPackRequestId,
            employee,
            merchPack,
            new Email(employeeNeededMerchEvent.HrEmail),
            employeeNeededMerchEvent.ClothingSize,
            RequestType.Auto);
        
        var command = new ReserveMerchPackCommand(merchPackRequest, employeeNeededMerchEvent.EventType);
            
        await mediator.Send(command, cancellationToken);
    }

    private MerchPackRequestDto GetDto(EmployeeNeededMerchEvent evnt, MerchPack merchPack, Employee employee)
    {
        var dto = new MerchPackRequestDto()
        {
            MerchpackTypeId = (int)evnt.MerchType,
            MerchPackItems = JsonSerializer.Serialize(merchPack.Items.Select(item => new
            {
                item.Type.ItemTypeId,
                item.Type.ItemTypeName,
                Sku = item.Sku.Value,
                Quantity = item.Quantity.Value
            })),
            EmployeeId = employee.Id,
            ClothingSizeId = (int)evnt.ClothingSize,
            HrEmail = evnt.HrEmail,
            RequestTypeId = evnt.RequestType.Id,
            RequestedAt = DateTimeOffset.UtcNow,
            RequestStatusId = RequestStatus.Created.Id,
            Issued = null
        };

        return dto;
    }
}