using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

/// <summary>
/// Get received merch packs query
/// </summary>
/// <param name="employeeId">Employee id</param>
/// <param name="employeeEmail">Employee email</param>
public class GetReceivedMerchPacksQuery(long employeeId, string employeeEmail) : IRequest<IEnumerable<MerchPack>>
{
    /// <summary>
    /// Employee id property
    /// </summary>
    public long EmployeeId { get; } = employeeId;
    /// <summary>
    /// Employee email property 
    /// </summary>
    public string EmployeeEmail { get; } = employeeEmail;
}