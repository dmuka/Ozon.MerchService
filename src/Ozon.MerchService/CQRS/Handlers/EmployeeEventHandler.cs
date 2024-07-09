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
        var merchData = employeeEvent.NotificationEvent.Payload as MerchDeliveryEventPayload;

        var employee = await employeeRepository.GetByEmailAsync(employeeEvent.NotificationEvent.EmployeeEmail, cancellationToken);
        
        var command = new ReserveMerchPackCommand()
        {
            EmployeeId = employee.Id,
            MerchPackType = merchData.MerchType
        };
            
        await mediator.Send(command, cancellationToken);
    }
}