using MediatR;
using Ozon.MerchService.Infrastructure.Extensions;

namespace Ozon.MerchService.Infrastructure.Mediatr;

/// <summary>
/// Handle logic to log commands handling logic in mediatr pipeline
/// </summary>
/// <param name="logger">Logger instance</param>
/// <typeparam name="TRequest">Type of request</typeparam>
/// <typeparam name="TResponse">Type of response</typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handle logic to log main logic in mediatr pipeline
    /// </summary>
    /// <param name="request">Request instance</param>
    /// <param name="next">Next handler in mediatr pipeline</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);
        var response = await next();
        logger.LogInformation("Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), response);

        return response;
    }
}