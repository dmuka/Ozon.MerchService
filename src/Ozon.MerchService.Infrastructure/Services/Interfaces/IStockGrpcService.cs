using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Infrastructure.Services.Interfaces;

public interface IStockGrpcService
{
    Task<bool> GetMerchPackItemsAvailability(MerchPackRequest merchPackRequest, CancellationToken token);
    Task<bool> ReserveMerchPackItems(MerchPackRequest merchPackRequest, CancellationToken token);
    void SetItemsSkusInRequest(MerchPackRequest merchPackRequest, ClothingSize clothingSize, CancellationToken token);
}