using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.HttpClient.Interfaces;

public interface IMerchServiceHttpClient
{
    Task<ReceivedMerchResponse> GetReceivedMerch(ReceivedMerchRequest receivedMerchRequest, CancellationToken cancellationToken);

    Task<RequestStatus> ReserveMerch(ReserveMerchRequest reserveMerchRequest, CancellationToken cancellationToken);
}