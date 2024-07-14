using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

public class GetReceivedMerchPacksQuery : IRequest<IEnumerable<MerchPack>>
{
    public GetReceivedMerchPacksQuery(long employeeId)
    {
        EmployeeId = employeeId;
    }
    
    public long EmployeeId { get; set; }
}