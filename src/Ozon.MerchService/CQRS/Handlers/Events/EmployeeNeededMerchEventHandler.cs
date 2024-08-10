using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers.Events;

public class EmployeeNeededMerchEventHandler(IMediator mediator)
    : INotificationHandler<EmployeeNeededMerchEvent>
{
    public async Task Handle(EmployeeNeededMerchEvent evnt, CancellationToken cancellationToken)
    {
        var command = new CreateMerchPackRequestCommand(
            evnt.EmployeeName,
            string.Empty,
            evnt.EmployeeEmail,
            evnt.HrEmail,
            evnt.HrName,
            evnt.MerchType,
            evnt.ClothingSize,
            RequestType.Auto);

        await mediator.Send(command, cancellationToken);
    }
}