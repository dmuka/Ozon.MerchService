using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.HttpClient.Interfaces;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.HttpClient.Implementations;

public class MerchServiceHttpClient : IMerchServiceHttpClient
{
    public Task<ReceivedMerchResponse> GetReceivedMerch(ReceivedMerchRequest receivedMerchRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MerchPack> ReserveMerch(ReserveMerchRequest reserveMerchRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}