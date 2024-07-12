using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Services.Implementations;

public class QuequedRequestsService(
    IMerchPackRequestRepository merchPackRequestRepository,
    IMediator mediator,
    ILogger<QuequedRequestsService> logger) : IQuequedRequestsService
{
    public async Task RepeatReserve(IEnumerable<long> skuCollection, CancellationToken token)
    {
        var quequedMerchPackRequests = await merchPackRequestRepository.GetByRequestStatusAsync(Status.Quequed, token);

        var replenishedMerchPackRequests = quequedMerchPackRequests
            .Where(request => request.MerchItems.Any(item => skuCollection.Contains(item.StockKeepingUnit)));
        
        foreach (var request in replenishedMerchPackRequests)
        {
            var command = new ReserveMerchPackCommand(request);
            
            await mediator.Send(command, token);
        }
    }
}