using OpenTelemetry.Trace;

namespace Ozon.MerchService.Infrastructure.Middlewares;

public class TracingMiddleware(RequestDelegate next, TracerProvider tracerProvider) : BaseMiddleware
{
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