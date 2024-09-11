using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ozon.MerchService.CQRS.Queries;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.Controllers.v1;

/// <summary>
/// Merch controller
/// </summary>
/// <param name="mediator">Mediatr object for DI</param>
[ApiController]
[Route("v1/api/merchpack")]
public class MerchPackController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<MerchPackDto>> GetMerchPackById(
        int merchPackId, 
        CancellationToken cancellationToken)
    {
        var query = new GetMerchPackByIdQuery(merchPackId);

        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<MerchPackDto>>> GetAllMerchPacks(CancellationToken cancellationToken)
    {
        var query = new GetAllMerchPacksQuery();

        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}