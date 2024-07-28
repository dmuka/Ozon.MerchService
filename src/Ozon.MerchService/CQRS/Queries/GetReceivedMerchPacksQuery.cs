using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

public class GetReceivedMerchPacksQuery(long employeeId, string employeeEmail) : IRequest<IEnumerable<MerchPack>>
{
    public long EmployeeId { get; set; } = employeeId;
    public string EmployeeEmail { get; set; } = employeeEmail;
}