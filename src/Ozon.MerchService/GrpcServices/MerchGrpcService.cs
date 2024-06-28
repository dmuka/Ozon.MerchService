using CSharpCourse.Core.Lib.Enums;
using Grpc.Core;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.GRPC;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.GrpcServices;

/// <summary>
/// GRPC merch service
/// </summary>
public class MerchGrpcService : GRPC.MerchService.MerchServiceBase
{
    private readonly IMerchService _merchService;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="merchService">Merch service</param>
    public MerchGrpcService(IMerchService merchService)
    {
        _merchService = merchService;
    }
    
    /// <summary>
    /// Request merch pack for employee
    /// </summary>
    /// <param name="request">Request model</param>
    /// <param name="context">Context model</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
    {
        if (request.EmployeeId <= 0)
        {
            throw new ArgumentException($"{nameof(RequestMerchRequest.EmployeeId)} can not be smaller than 1");
        }
            
        if ((int)request.MerchPackId is <= (int)MerchType.WelcomePack or >= (int)MerchType.VeteranPack)
        {
            throw new ArgumentException($"{nameof(RequestMerchRequest.MerchPackId)} is unknown");
        }

        var merchPack = new MerchPack((MerchType)request.MerchPackId);

        await _merchService.ReserveMerchAsync(request.EmployeeId, merchPack, context.CancellationToken);
            
        return new RequestMerchResponse()
        {
            MerchId = (int)merchPack.MerchPackType
        };
    }
}