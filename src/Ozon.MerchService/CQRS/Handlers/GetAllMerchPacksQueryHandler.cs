using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Helpers;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler to get all merch packs
/// </summary>
/// <param name="repository">Merch pack repository</param>
/// <param name="tracerProvider">Tracer provider</param>
public class GetAllMerchPacksQueryHandler(
    IMerchPacksRepository repository, 
    TracerProvider tracerProvider) : IRequestHandler<GetAllMerchPacksQuery, IEnumerable<MerchPackDto>>
{
    /// <summary>
    /// Gets all merch packs
    /// </summary>
    /// <param name="request">Query with merch pack id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<IEnumerable<MerchPackDto>> Handle(GetAllMerchPacksQuery request, CancellationToken cancellationToken)
    {
        const string handlerName = nameof(GetMerchPackByIdQueryHandler);
        
        var tracer = tracerProvider.GetTracer(handlerName);

        using var span = Tracing.StartSpan(tracer, handlerName, SpanKind.Internal);

        var merchPacks = await repository.GetAllAsync<MerchPackDto>(cancellationToken);

        return merchPacks;
    }
}