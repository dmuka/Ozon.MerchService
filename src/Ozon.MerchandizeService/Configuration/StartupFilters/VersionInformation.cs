using Ozon.MerchandizeService.Configuration.Constants;
using Ozon.MerchandizeService.Configuration.Middlewares;

namespace Ozon.MerchandizeService.Configuration.StartupFilters;

/// <summary>
/// Application version startup filter
/// </summary>
public class VersionInformation : IStartupFilter
{
    /// <summary>
    /// Return application version
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
        
            application.Map(
                Routes.VersionInformation, 
                configuration => configuration.UseMiddleware<ApplicationVersionMiddleware>());
            
            next(application);
        };
    }
}