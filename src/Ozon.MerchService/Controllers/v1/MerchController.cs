using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.HttpModels;
using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService.Controllers.v1;

[ApiController]
[Route("/api/merch")]
public class MerchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("received")]
    public async Task<ActionResult<IEnumerable<MerchPack>>> GetReceivedMerch(
        ReceivedMerchRequest receivedMerchRequest, 
        CancellationToken cancellationToken)
    {
        var query = new GetReceivedMerchPacksQuery(receivedMerchRequest.EmployeeId);

        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("reserve")]
    public async Task<ActionResult<MerchPack>> ReserveMerch(
        ReserveMerchRequest reserveMerchRequest, 
        CancellationToken cancellationToken)
    {
        var command = new ReserveMerchPackCommand(
            reserveMerchRequest,
            EmployeeEventType.MerchDelivery,
            Status.Created,
            RequestType.Manual);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}