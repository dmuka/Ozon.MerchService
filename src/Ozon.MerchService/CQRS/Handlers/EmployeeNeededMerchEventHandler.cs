using CSharpCourse.Core.Lib.Events;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.CQRS.Handlers;

public class EmployeeNeededMerchEventHandler(IEmployeeRepository employeeRepository, IMediator mediator) : INotificationHandler<EmployeeNeededMerchEvent>
{
    public async Task Handle(EmployeeNeededMerchEvent employeeNeededMerchEvent, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByEmailAsync(employeeNeededMerchEvent.EmployeeEmail, cancellationToken);
        
        var command = new ReserveMerchPackCommand(
            employeeNeededMerchEvent.EventType,
            default(long),
            employeeNeededMerchEvent.EmployeeName,
            "",
            employeeNeededMerchEvent.EmployeeEmail,
            employeeNeededMerchEvent.HrEmail,
            employeeNeededMerchEvent.ClothingSize,
            employeeNeededMerchEvent.MerchType);
            
        await mediator.Send(command, cancellationToken);
    }
}