using MediatR;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Create merch pack request command
/// </summary>
/// <param name="merchPackName">Merch pack name</param>
/// <param name="merchItemDtos">Merch pack items collection</param>
public class CreateMerchPackCommand(string merchPackName, List<MerchItemDto> merchItems) : IRequest<int>
{
    /// <summary>
    /// Merch pack name property
    /// </summary>
    public string MerchPackName { get; private set; } = merchPackName;
    
    /// <summary>
    /// Merch pack id property
    /// </summary>
    public List<MerchItemDto> MerchItems { get; private set; } = merchItems;
}