using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class EmployeeNeededMerchEventHandler(
    IMediator mediator,  
    IUnitOfWork unitOfWork,
    IEmployeeRepository employeeRepository,
    IMerchPacksRepository merchPacksRepository)
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
                String.Empty, 
                employeeNeededMerchEvent.EmployeeEmail);
        }
        var merchPack = await merchPacksRepository.GetMerchPackById((int)employeeNeededMerchEvent.MerchType, cancellationToken);
        
        var merchPackRequest = new MerchPackRequest(
            merchPack,
            employeeNeededMerchEvent.ClothingSize,
            employee,
            new Email(employeeNeededMerchEvent.HrEmail),
            employeeNeededMerchEvent.RequestType);
        
        var command = new ReserveMerchPackCommand(merchPackRequest, employeeNeededMerchEvent.EventType);
            
        await mediator.Send(command, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}