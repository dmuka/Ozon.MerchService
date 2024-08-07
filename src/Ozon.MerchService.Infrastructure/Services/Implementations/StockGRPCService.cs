using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Services.Implementations;

public class StockGrpcService(StockApiGrpc.StockApiGrpcClient stockClient) : IStockGrpcService
{
    /// <summary>
    /// Set sku values in merch pack items of the request
    /// </summary>
    /// <param name="merchItems">Merch pack items</param>
    /// <param name="clothingSize">Employee clothing size</param>
    /// <param name="token">Cancellation token</param>
    public void SetItemsSkusInRequest(
        IList<MerchItem> merchItems, 
        ClothingSize clothingSize, 
        CancellationToken token)
    {
        foreach (var merchItem in merchItems)
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

    /// <summary>
    /// Reserve merch pack items if they are available on stock
    /// </summary>
    /// <param name="merchItems">Merch pack items collection</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>True if reserve, false - otherwise</returns>
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
                Quantity = item.Quantity.Value
            });
            
        request.Items.AddRange(skuQuantityItems);

        try
        {
            var response = await stockClient.GiveOutItemsAsync(request, null, null, cancellationToken: token);

            return response.Result == GiveOutItemsResponse.Types.Result.Successful;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task<bool> GetMerchPackItemsAvailability(IList<MerchItem> merchItems, CancellationToken token)
    {
        var requestSkus = merchItems.Select(item => item.Sku.Value);
        
        var request = new SkusRequest();
        request.Skus.AddRange(requestSkus);
                
        var response = await stockClient.GetStockItemsAvailabilityAsync(request, cancellationToken: token);

        var allItemsAvailable = response.Items.All(stockItem =>
        {
            var merchItem = merchItems.First(merchItem => merchItem.Sku.Value == stockItem.Sku);

            return stockItem.Quantity >= merchItem.Quantity.Value;
        });
        
        return allItemsAvailable;
    }
}