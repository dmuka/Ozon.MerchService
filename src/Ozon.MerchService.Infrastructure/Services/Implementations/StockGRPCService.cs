using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Services.Implementations;

public class StockGrpcService(StockApiGrpc.StockApiGrpcClient stockClient) : StockApiGrpc.StockApiGrpcBase
{
    public async Task<bool> GetMerchPackItemsAvailability(MerchPackRequest merchPackRequest, CancellationToken token)
    {
        var requestSkus = merchPackRequest.MerchItems.Select(item => item.StockKeepingUnit);
        
        var request = new SkusRequest();
        request.Skus.AddRange(requestSkus);
                
        var response = await stockClient.GetStockItemsAvailabilityAsync(request, cancellationToken: token);

        var allItemsAvailable = response.Items.All(i => i.Quantity > 0);
        
        return allItemsAvailable;
    }

    public async Task<bool> ReserveMerchPackItems(MerchPackRequest merchPackRequest, CancellationToken token)
    {
        var request = new GiveOutItemsRequest();
            
        var skuQuantityItems = merchPackRequest.MerchItems
            .Select(item => new SkuQuantityItem
            {
                Sku = item.StockKeepingUnit,
                Quantity = 1
            });
            
        request.Items.AddRange(skuQuantityItems);

        var response = await stockClient.GiveOutItemsAsync(request, cancellationToken: token);

        return response.Result == GiveOutItemsResponse.Types.Result.Successful;
    }
}