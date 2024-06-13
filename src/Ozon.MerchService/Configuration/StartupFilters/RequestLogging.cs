using Ozon.MerchService.Configuration.Middlewares;

namespace Ozon.MerchService.Configuration.StartupFilters;

/// <summary>
/// Request logging startup filter
/// </summary>
public class RequestLogging : IStartupFilter
{
    /// <summary>
    /// Log request middleware 
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
            application.UseMiddleware<RequestLoggingMiddleware>();
            next(application);
        };
    }
}