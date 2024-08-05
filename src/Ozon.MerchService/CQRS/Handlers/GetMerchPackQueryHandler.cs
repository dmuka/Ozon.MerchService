using MediatR;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Helpers;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

public class GetMerchPackQueryHandler(
    IMerchPacksRepository merchPacksRepository,
    IStockGrpcService stockGrpcService,
    TracerProvider tracerProvider)
    : IRequestHandler<GetMerchPackQuery, MerchPack>
{
    public async Task<MerchPack> Handle(GetMerchPackQuery query, CancellationToken cancellationToken)
    {
        var tracer = tracerProvider.GetTracer("GetMerchPackQueryHandler");

        using var span = Tracing.StartSpan(tracer, "HandleGetMerchPackQueryHandler", SpanKind.Internal,
            new Dictionary<string, object>
            {
                { "item.id", query.MerchPackType }
            });
        
        var merchPack = await merchPacksRepository.GetMerchPackById((int)query.MerchPackType, cancellationToken);
            
        stockGrpcService.SetItemsSkusInRequest(merchPack.Items.ToList(), query.ClothingSize, cancellationToken);

        return merchPack;
    }
}