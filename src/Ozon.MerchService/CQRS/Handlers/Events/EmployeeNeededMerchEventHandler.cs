using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class EmployeeNeededMerchEventHandler(
    IMediator mediator,  
    IUnitOfWork unitOfWork,
    IEmployeeRepository employeeRepository)
    : INotificationHandler<EmployeeNeededMerchEvent>
{
    public async Task Handle(EmployeeNeededMerchEvent employeeNeededMerchEvent, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);    
        
        var employee = await employeeRepository.GetByEmailAsync(employeeNeededMerchEvent.EmployeeEmail, cancellationToken);

        if (employee is null)
        {
            var empl = new Employee(new FullName(employeeNeededMerchEvent.EmployeeName),
                new Email(employeeNeededMerchEvent.EmployeeEmail));
            
            var employeeId = await employeeRepository.CreateAsync(empl, cancellationToken, new { FullName = empl.FullName.Value, Email = empl.Email.Value });
            
            employee = Employee.CreateInstance(employeeId, empl.FullName.Value, empl.Email.Value);
        }
        
        var merchPackRequest = new MerchPackRequest(
            employeeNeededMerchEvent.MerchType,
            employeeNeededMerchEvent.ClothingSize,
            employee,
            employeeNeededMerchEvent.RequestType);
        
        var command = new ReserveMerchPackCommand(merchPackRequest);
            
        await mediator.Send(command, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}