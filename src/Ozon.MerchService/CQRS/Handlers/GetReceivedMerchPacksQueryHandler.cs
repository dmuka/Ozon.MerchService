using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Helpers;

namespace Ozon.MerchService.CQRS.Handlers;

public class GetReceivedMerchPacksQueryHandler(
    IMerchPackRequestRepository merchPackRequestRepository, 
    IUnitOfWork unitOfWork,
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
        
        await unitOfWork.StartTransaction(cancellationToken);

        var merchPacks =
            await merchPackRequestRepository.GetByMerchPacksByEmployeeIdAsync(request.EmployeeId,
                cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return merchPacks;
    }
}