using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class EmployeeNeededMerchEventHandler(IEmployeeRepository employeeRepository, IMediator mediator)
    : INotificationHandler<EmployeeNeededMerchEvent>
{
    public async Task Handle(EmployeeNeededMerchEvent employeeNeededMerchEvent, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByEmailAsync(employeeNeededMerchEvent.EmployeeEmail, cancellationToken);

        var merchPackRequest = new MerchPackRequest(
            employeeNeededMerchEvent.MerchType,
            employee,
            employeeNeededMerchEvent.RequestType);
        
        var command = new ReserveMerchPackCommand(merchPackRequest);
            
        await mediator.Send(command, cancellationToken);
    }
}