using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Helpers;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler to get merch pack of certain type with items skus
/// </summary>
/// <param name="merchPacksRepository">Merch pack repository</param>
/// <param name="stockGrpcService">Stock grpc service</param>
/// <param name="tracerProvider">Tracer provider</param>
public class GetMerchPackQueryHandler(
    IMerchPacksRepository merchPacksRepository,
    IStockGrpcService stockGrpcService,
    TracerProvider tracerProvider)
    : IRequestHandler<GetMerchPackQuery, MerchPack>
{
    /// <summary>
    /// Gets merch pack of certain type
    /// </summary>
    /// <param name="query">Query with merch pack type data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<MerchPack> Handle(GetMerchPackQuery query, CancellationToken cancellationToken)
    {
        const string handlerName = nameof(GetMerchPackQueryHandler);
        
        var tracer = tracerProvider.GetTracer(handlerName);

        using var span = Tracing.StartSpan(tracer, handlerName, SpanKind.Internal,
            new Dictionary<string, object>
            {
                { "item.id", query.MerchPackType }
            });
        
        var merchPack = await merchPacksRepository.GetMerchPackById((int)query.MerchPackType, cancellationToken);
            
        stockGrpcService.SetItemsSkusInRequest(merchPack.Items, query.ClothingSize, cancellationToken);

        return merchPack;
    }
}