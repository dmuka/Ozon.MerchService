using Ozon.MerchandizeService.Configuration.Constants;
using Ozon.MerchandizeService.Configuration.Middlewares;

namespace Ozon.MerchandizeService.Configuration.StartupFilters;

/// <summary>
/// Live response startup filter
/// </summary>
public class LiveResponse : IStartupFilter
{
    /// <summary>
    /// Add live middleware 
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
        
            application.Map(
                Routes.LiveResponse, 
                configuration => configuration.UseMiddleware<LiveMiddleware>());
            
            next(application);
        };
    }
}