using Microsoft.AspNetCore.Mvc;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.HttpModels;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Controllers.v1;

[ApiController]
[Route("/api/merch")]
public class MerchController(IMerchService merchService) : ControllerBase
{
    [HttpGet]
    [Route("received")]
    public async Task<ActionResult<List<MerchPack>>> GetReceivedMerch(
        ReceivedMerchRequest receivedMerchRequest, 
        CancellationToken cancellationToken)
    {
        var packs = await merchService.GetReceivedMerchAsync(receivedMerchRequest.EmployeeId, cancellationToken);

        return Ok(packs);
    }
    
    [HttpGet]
    [Route("reserve")]
    public async Task<ActionResult<MerchPack>> ReserveMerch(
        ReserveMerchRequest reserveMerchRequest, 
        CancellationToken cancellationToken)
    {

        var merchPack = new MerchPack(reserveMerchRequest.MerchPackType, reserveMerchRequest.ClothingSize);
        
        var merch = await merchService.ReserveMerchAsync(reserveMerchRequest.EmployeeId, merchPack, cancellationToken);

        if (merch is null) return NotFound();

        return Ok(merch);
    }
}