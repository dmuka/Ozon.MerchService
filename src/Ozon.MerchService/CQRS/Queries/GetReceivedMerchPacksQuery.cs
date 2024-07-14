using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

public class GetReceivedMerchPacksQuery : IRequest<List<MerchPack>>
{
    public long EmployeeId { get; set; }
}