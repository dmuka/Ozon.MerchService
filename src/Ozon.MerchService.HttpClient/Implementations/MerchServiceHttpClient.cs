using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.HttpClient.Interfaces;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.HttpClient.Implementations;

public class MerchServiceHttpClient(IMediator mediator) : IMerchServiceHttpClient
{
    public async Task<ReceivedMerchResponse> GetReceivedMerch(ReceivedMerchRequest receivedMerchRequest, CancellationToken cancellationToken)
    {
        var query = new GetReceivedMerchPacksQuery(receivedMerchRequest.EmployeeId, receivedMerchRequest.EmployeeEmail);

        var result = await mediator.Send(query, cancellationToken);
        
        return new ReceivedMerchResponse { MerchPacks = result.ToList() };
    }

    public async Task<RequestStatus> ReserveMerch(ReserveMerchRequest reserveMerchRequest, CancellationToken cancellationToken)
    {
        
        var command = new CreateMerchPackRequestCommand(
            reserveMerchRequest.EmployeeFirstName,
            reserveMerchRequest.EmployeeLastName,
            reserveMerchRequest.EmployeeEmail,
            reserveMerchRequest.HrEmail,
            reserveMerchRequest.HrName,
            reserveMerchRequest.MerchPackType,
            reserveMerchRequest.ClothingSize,
            RequestType.Auto);

        var result = await mediator.Send(command, cancellationToken);

        return result;
    }
}