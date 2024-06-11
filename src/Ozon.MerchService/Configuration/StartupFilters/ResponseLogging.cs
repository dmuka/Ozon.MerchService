using Ozon.MerchandizeService.Configuration.Middlewares;

namespace Ozon.MerchandizeService.Configuration.StartupFilters;

/// <summary>
/// Response logging startup filter
/// </summary>
public class ResponseLogging : IStartupFilter
{
    /// <summary>
    /// Log response middleware 
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
            application.UseMiddleware<ResponseLoggingMiddleware>();
            next(application);
        };
    }
}