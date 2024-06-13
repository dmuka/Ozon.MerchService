using System.Reflection;
using Ozon.MerchService.Configuration.Constants;

namespace Ozon.MerchService.Configuration.Middlewares;

/// <summary>
/// Middleware for application version information
/// </summary>
public class ApplicationVersionMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Write application version information in response
    /// </summary>
    /// <param name="context">Http context object</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var applicationVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "No version";

        await context.Response.WriteAsync($"version: {applicationVersion}, serviceName: {Names.GetApplicationName()}");
    }
}