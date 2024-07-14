using MediatR;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers;

public class GetReceivedMerchPacksQueryHandler(IMerchPackRequestRepository merchPackRequestRepository)
    : IRequestHandler<GetReceivedMerchPacksQuery, IEnumerable<MerchPack>>
{
    public Task<IEnumerable<MerchPack>> Handle(GetReceivedMerchPacksQuery request, CancellationToken cancellationToken)
    {
        var merchPacks =
            merchPackRequestRepository.GetByMerchPacksByEmployeeIdAsync(request.EmployeeId, cancellationToken);

        return merchPacks;
    }
}