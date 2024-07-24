using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;
using Ozon.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Services.Implementations;

public class StockGrpcService(StockApiGrpc.StockApiGrpcClient stockClient) : IStockGrpcService
{
    public async Task<bool> GetMerchPackItemsAvailability(MerchPackRequest merchPackRequest, CancellationToken token)
    {
        var requestSkus = merchPackRequest.MerchItems.Select(item => item.Sku.Value);
        
        var request = new SkusRequest();
        request.Skus.AddRange(requestSkus);
                
        var response = await stockClient.GetStockItemsAvailabilityAsync(request, cancellationToken: token);

        var allItemsAvailable = response.Items.All(i => i.Quantity > 0);
        
        return allItemsAvailable;
    }
    
    public void SetItemsSkusInRequest(
        MerchPackRequest merchPackRequest, 
        ClothingSize clothingSize, 
        CancellationToken token)
    {
        foreach (var merchItem in merchPackRequest.MerchItems)
        {
            var id = merchItem.Type.Id;
            
            var model = new IntIdModel { Id = id };
            
            var items = stockClient.GetByItemType(model, null, null, token).Items;

            if (items.Count > 1)
            {
                var item = items.FirstOrDefault(unit => unit.SizeId == (int)clothingSize);

                if (item is null) throw new ArgumentException("No sku with such clothing size found.");
                
                merchItem.SetSku(item.Sku);
            }
            else
            {
                merchItem.SetSku(items.First().Sku);
            }            
        }
    }

    public async Task<bool> ReserveMerchPackItems(MerchPackRequest merchPackRequest, CancellationToken token)
    {
        var request = new GiveOutItemsRequest();
            
        var skuQuantityItems = merchPackRequest.MerchItems
            .Select(item => new SkuQuantityItem
            {
                Sku = item.Sku.Value,
                Quantity = 1
            });
            
        request.Items.AddRange(skuQuantityItems);

        var response = await stockClient.GiveOutItemsAsync(request, cancellationToken: token);

        return response.Result == GiveOutItemsResponse.Types.Result.Successful;
    }
}