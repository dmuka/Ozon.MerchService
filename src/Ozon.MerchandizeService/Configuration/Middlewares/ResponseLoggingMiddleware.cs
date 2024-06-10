using System.Text;

namespace Ozon.MerchandizeService.Configuration.Middlewares;

/// <summary>
/// Middleware for logging of responses
/// </summary>
public class ResponseLoggingMiddleware(RequestDelegate next, ILogger<ResponseLoggingMiddleware> logger) : BaseMiddleware
{
    /// <summary>
    /// Log request info
    /// </summary>
    /// <param name="context">Http context object</param>
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        
        var message = GetRouteHeadersLogMessage(context.Request.Path, context.Response.Headers);
        
        logger.LogInformation(message);
        
    }
}