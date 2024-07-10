using CSharpCourse.Core.Lib.Events;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.CQRS.Handlers;

public class EmployeeEventHandler(IEmployeeRepository employeeRepository, IMediator mediator) : INotificationHandler<EmployeeEvent>
{
    public async Task Handle(EmployeeEvent employeeEvent, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByEmailAsync(employeeEvent.EmployeeEmail, cancellationToken);
        
        var command = new ReserveMerchPackCommand(
            employeeEvent.EventType,
            default(long),
            employeeEvent.EmployeeName,
            "",
            employeeEvent.EmployeeEmail,
            employeeEvent.HrEmail,
            employeeEvent.ClothingSize,
            employeeEvent.MerchType);
            
        await mediator.Send(command, cancellationToken);
    }
}