using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Helpers;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler for received merch packs query
/// </summary>
/// <param name="employeeRepository">Repository of employees</param>
/// <param name="tracerProvider">Tracer provider</param>
public class GetReceivedMerchPacksQueryHandler(
    IEmployeeRepository employeeRepository,
    TracerProvider tracerProvider)
    : IRequestHandler<GetReceivedMerchPacksQuery, IEnumerable<MerchPack>>
{
    /// <summary>
    /// Handle for received merch packs query
    /// </summary>
    /// <param name="query">Received merch packs query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<IEnumerable<MerchPack>> Handle(GetReceivedMerchPacksQuery query, CancellationToken cancellationToken)
    {
        var tracer = tracerProvider.GetTracer("GetReceivedMerchPacksQueryHandler");

        using var span = Tracing.StartSpan(tracer, "HandleGetReceivedMerchPacksQuery", SpanKind.Internal,
            new Dictionary<string, object>
            {
                { "item.id", query.EmployeeId }
            });

        var employee = await employeeRepository.GetByEmailAsync(query.EmployeeEmail, cancellationToken);

        if (employee is null) return Array.Empty<MerchPack>();
        
        var merchPacks = employee.MerchPacksRequests.Select(merchPackRequest =>
            new MerchPack(merchPackRequest.MerchPack.MerchPackType, merchPackRequest.MerchPack.Items));

        return merchPacks;
    }
}