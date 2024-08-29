using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.HttpModels;

namespace Ozon.MerchService.Controllers.v1;

/// <summary>
/// Merch controller
/// </summary>
/// <param name="mediator">Mediatr object for DI</param>
[ApiController]
[Route("v1/api/merch")]
public class MerchController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Return merch packs received by employee
    /// </summary>
    /// <param name="receivedMerchRequest">Request object with employee id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection with received merch packs or empty collection</returns>
    [HttpGet]
    [Route("received")]
    public async Task<ActionResult<IEnumerable<MerchPack>>> GetReceivedMerch(
        [FromQuery] ReceivedMerchRequest receivedMerchRequest, 
        CancellationToken cancellationToken)
    {
        var query = new GetReceivedMerchPacksQuery(receivedMerchRequest.EmployeeId, receivedMerchRequest.EmployeeEmail);

        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Reserve merch pack for employee
    /// </summary>
    /// <param name="request">Request object with employee id and merch pack type id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reserved merch pack object</returns>
    [HttpPost]
    [Route("reserve")]
    public async Task<ActionResult<RequestStatus>> ReserveMerch(
        [FromBody] ReserveMerchRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateMerchPackRequestCommand(
            request.EmployeeFirstName,
            request.EmployeeLastName,
            request.EmployeeEmail,
            request.HrEmail,
            request.HrName,
            request.MerchPackType,
            request.ClothingSize,
            RequestType.Auto);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}