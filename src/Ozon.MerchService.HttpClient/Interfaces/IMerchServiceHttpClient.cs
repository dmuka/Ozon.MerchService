using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.HttpClient.Interfaces;

public interface IMerchServiceHttpClient
{
    Task<ReceivedMerchResponse> GetReceivedMerch(ReceivedMerchRequest receivedMerchRequest, CancellationToken cancellationToken);

    Task<MerchPack> ReserveMerch(ReserveMerchRequest reserveMerchRequest, CancellationToken cancellationToken);
}