using Ozon.MerchandizeService.Configuration.Constants;
using Ozon.MerchandizeService.Configuration.Middlewares;

namespace Ozon.MerchandizeService.Configuration.StartupFilters;

/// <summary>
/// Ready response startup filter
/// </summary>
public class ReadyResponse : IStartupFilter
{
    /// <summary>
    /// Add ready middleware 
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
            next(application);
            
            application.Map(
                Routes.ReadyResponse, 
                configuration => configuration.UseMiddleware<ReadyMiddleware>());
        };
    }
}