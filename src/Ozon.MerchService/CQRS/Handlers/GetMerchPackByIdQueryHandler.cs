using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Helpers;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler to get merch pack by id
/// </summary>
/// <param name="repository">Merch pack repository</param>
/// <param name="tracerProvider">Tracer provider</param>
public class GetMerchPackByIdQueryHandler(
    IMerchPacksRepository repository, 
    TracerProvider tracerProvider) : IRequestHandler<GetMerchPackByIdQuery, MerchPackDto?>
{
    /// <summary>
    /// Gets merch pack by id
    /// </summary>
    /// <param name="request">Query with merch pack id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<MerchPackDto?> Handle(GetMerchPackByIdQuery request, CancellationToken cancellationToken)
    {
        const string handlerName = nameof(GetMerchPackByIdQueryHandler);
        
        var tracer = tracerProvider.GetTracer(handlerName);

        using var span = Tracing.StartSpan(tracer, handlerName, SpanKind.Internal,
            new Dictionary<string, object>
            {
                { "item.id", request.MerchPackId }
            });
        
        var merchPackId = request.MerchPackId;

        var merchPack = await repository.GetByIdAsync<MerchPackDto, int>(merchPackId, cancellationToken);

        return merchPack;
    }
}