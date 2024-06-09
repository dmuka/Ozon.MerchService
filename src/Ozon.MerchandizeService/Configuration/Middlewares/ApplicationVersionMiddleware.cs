using System.Reflection;
using Ozon.MerchandizeService.Configuration.Constants;

namespace Ozon.MerchandizeService.Configuration.Middlewares;

/// <summary>
/// Middleware for application version information
/// </summary>
public class ApplicationVersionMiddleware
{
    public ApplicationVersionMiddleware(RequestDelegate next) {}

    /// <summary>
    /// Write application version information in response
    /// </summary>
    /// <param name="context">Http context object</param>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var applicationVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "No version";

        await context.Response.WriteAsync($"version: {applicationVersion}, serviceName: {Names.GetApplicationName()}");
    }
}