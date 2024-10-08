using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;

namespace Ozon.MerchService.Infrastructure.Services.Interfaces;

public interface IStockGrpcService
{
    Task<bool> TryReserveMerchPackItems(IList<MerchItem> merchItems, CancellationToken token);
    void SetItemsSkusInRequest(IList<MerchItem> merchItems, ClothingSize clothingSize, CancellationToken token);
}