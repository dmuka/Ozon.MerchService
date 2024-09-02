using MediatR;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Queries;

/// <summary>
/// Get all merch packs
/// </summary>
public class GetAllMerchPacksQuery : IRequest<IEnumerable<MerchPackDto>>
{
}