using Grpc.Core;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.GRPC;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.GrpcServices;

public class MerchGrpcService : GRPC.MerchService.MerchServiceBase
{
    private readonly IMerchService _merchService;
    
    public MerchGrpcService(IMerchService merchService)
    {
        _merchService = merchService;
    }
    
    public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
    {
        if (request.EmployeeId <= 0)
        {
            throw new ArgumentException($"{nameof(RequestMerchRequest.EmployeeId)} can not be smaller than 1");
        }
            
        if ((int)request.MerchPackId is <= (int)MerchPackType.WelcomePack or >= (int)MerchPackType.VeteranPack)
        {
            throw new ArgumentException($"{nameof(RequestMerchRequest.MerchPackId)} is unknown");
        }

        var merchPack = new MerchPack((MerchPackType)request.MerchPackId);

        await _merchService.ReserveMerchAsync(request.EmployeeId, merchPack, context.CancellationToken);
            
        return new RequestMerchResponse()
        {
            MerchId = (int)merchPack.MerchPackType
        };
    }
}