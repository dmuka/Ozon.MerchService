using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.HttpModels;

public record CreateMerchPackRequest(string MerchPackName, List<MerchItemDto> MerchItems);