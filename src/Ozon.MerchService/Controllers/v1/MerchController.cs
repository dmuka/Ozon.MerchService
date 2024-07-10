using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.HttpModels;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Controllers.v1;

[ApiController]
[Route("/api/merch")]
public class MerchController(IMerchService merchService, IMediator mediator) : ControllerBase
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

        var command = new ReserveMerchPackCommand(
            null,
            reserveMerchRequest.EmployeeId,
            reserveMerchRequest.EmployeeFirstName,
            reserveMerchRequest.EmployeeLastName,
            reserveMerchRequest.EmployeeEmail,
            reserveMerchRequest.HrEmail,
            reserveMerchRequest.ClothingSize,
            reserveMerchRequest.MerchPackType);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}