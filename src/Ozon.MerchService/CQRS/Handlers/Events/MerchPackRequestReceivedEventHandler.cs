using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Events.Domain;

namespace Ozon.MerchService.CQRS.Handlers.Events;

/// <summary>
/// Handler for received merch pack request event
/// </summary>
/// <param name="mediator">Mediator instance</param>
public class MerchPackRequestReceivedEventHandler(IMediator mediator) : INotificationHandler<MerchPackRequestReceivedEvent>
{
    /// <summary>
    /// Handle for received merch pack request event
    /// </summary>
    /// <param name="evnt">Received merch pack request event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Handle(MerchPackRequestReceivedEvent evnt, CancellationToken cancellationToken)
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