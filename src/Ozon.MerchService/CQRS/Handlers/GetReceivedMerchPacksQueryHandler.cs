using MediatR;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.CQRS.Handlers;

public class GetReceivedMerchPacksQueryHandler(IMerchPackRequestRepository merchPackRequestRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<GetReceivedMerchPacksQuery, IEnumerable<MerchPack>>
{
    public async Task<IEnumerable<MerchPack>> Handle(GetReceivedMerchPacksQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.StartTransaction(cancellationToken);
        
        var merchPacks =
            await merchPackRequestRepository.GetByMerchPacksByEmployeeIdAsync(request.EmployeeId, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return merchPacks;
    }
}