using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ozon.MerchService.Configuration.Constants;

namespace Ozon.MerchService.Infrastructure.Configuration.Middlewares;

/// <summary>
/// Middleware for logging of requests
/// </summary>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger) : BaseMiddleware
{
    /// <summary>
    /// Log request info
    /// </summary>
    /// <param name="context">Http context object</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType != ContentTypes.Grpc)
        {
            var message = GetRouteHeadersLogMessage(context.Request.Path, context.Request.Headers);

            logger.LogInformation(message);
        }

        await next(context);
    }
}