using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Helpers;

namespace Ozon.MerchService.CQRS.Handlers;

public class GetReceivedMerchPacksQueryHandler(
    IMerchPackRequestRepository merchPackRequestRepository, 
    IEmployeeRepository employeeRepository,
    TracerProvider tracerProvider)
    : IRequestHandler<GetReceivedMerchPacksQuery, IEnumerable<MerchPack>>
{
    public async Task<IEnumerable<MerchPack>> Handle(GetReceivedMerchPacksQuery request, CancellationToken cancellationToken)
    {
        var tracer = tracerProvider.GetTracer("GetReceivedMerchPacksQueryHandler");

        using var span = Tracing.StartSpan(tracer, "HandleGetReceivedMerchPacksQuery", SpanKind.Internal,
            new Dictionary<string, object>
            {
                { "item.id", request.EmployeeId }
            });

        var employee = await employeeRepository.GetByEmailAsync(request.EmployeeEmail, cancellationToken);

        if (employee is null) return Array.Empty<MerchPack>();
        
        var merchPacks = employee.MerchPacksRequests.Select(merchPackRequest =>
            new MerchPack(merchPackRequest.MerchPackType, merchPackRequest.MerchItems));

        return merchPacks;
    }
}