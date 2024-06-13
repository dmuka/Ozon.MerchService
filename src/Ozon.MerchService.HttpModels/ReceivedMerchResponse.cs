using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.HttpModels;

public class ReceivedMerchResponse
{
    public List<MerchPack> MerchPacks { get; set; }
}