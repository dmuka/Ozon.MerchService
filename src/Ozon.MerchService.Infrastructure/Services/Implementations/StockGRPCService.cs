using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Services.Implementations;

public class StockGrpcService(StockApiGrpc.StockApiGrpcClient stockClient) : IStockGrpcService
{
    public void SetItemsSkusInRequest(
        MerchPackRequest merchPackRequest, 
        ClothingSize clothingSize, 
        CancellationToken token)
    {
        foreach (var merchItem in merchPackRequest.MerchItems)
        {
            var id = merchItem.Type.ItemTypeId;
            
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

    public async Task<bool> TryReserveMerchPackItems(IList<MerchItem> merchItems, CancellationToken token)
    {
        if (!await GetMerchPackItemsAvailability(merchItems, token))
        {
            return false;
        }
        
        var request = new GiveOutItemsRequest();
            
        var skuQuantityItems = merchItems
            .Select(item => new SkuQuantityItem
            {
                Sku = item.Sku.Value,
                Quantity = 1
            });
            
        request.Items.AddRange(skuQuantityItems);

        var response = await stockClient.GiveOutItemsAsync(request, cancellationToken: token);

        return response.Result == GiveOutItemsResponse.Types.Result.Successful;
    }
    
    private async Task<bool> GetMerchPackItemsAvailability(IList<MerchItem> merchItems, CancellationToken token)
    {
        var requestSkus = merchItems.Select(item => item.Sku.Value);
        
        var request = new SkusRequest();
        request.Skus.AddRange(requestSkus);
                
        var response = await stockClient.GetStockItemsAvailabilityAsync(request, cancellationToken: token);

        var allItemsAvailable = response.Items.All(i => i.Quantity > 0);
        
        return allItemsAvailable;
    }
}