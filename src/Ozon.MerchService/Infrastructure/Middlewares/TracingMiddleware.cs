using OpenTelemetry.Trace;

namespace Ozon.MerchService.Infrastructure.Middlewares;

/// <summary>
/// Tracing middleware for requests 
/// </summary>
/// <param name="next">Next middleware in pipeline</param>
/// <param name="tracerProvider">Tracer provider</param>
public class TracingMiddleware(RequestDelegate next, TracerProvider tracerProvider) : BaseMiddleware
{
    /// <summary>
    /// Add information about request in tracing span
    /// </summary>
    /// <param name="context">Http context of request</param>
    public async Task InvokeAsync(HttpContext context)
    {
        using (var span = tracerProvider.GetTracer("TracingMiddleware").StartActiveSpan("ProcessRequest"))
        {
            span.SetAttribute("http.method", context.Request.Method);
            span.SetAttribute("http.url", context.Request.Path);

            await next(context);

            span.SetAttribute("http.status_code", context.Response.StatusCode);
        }
    }
}