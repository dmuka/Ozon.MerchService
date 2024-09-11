using MediatR;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Queries;

/// <summary>
/// Get merch pack by id query
/// </summary>
/// <param name="merchPackId">Merch pack id</param>
public class GetMerchPackByIdQuery(int merchPackId) : IRequest<MerchPackDto>
{
    /// <summary>
    /// Merch pack id
    /// </summary>
    public int MerchPackId { get; } = merchPackId;
}