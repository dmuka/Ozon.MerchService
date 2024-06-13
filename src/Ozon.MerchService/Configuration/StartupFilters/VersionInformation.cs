using Ozon.MerchService.Configuration.Constants;
using Ozon.MerchService.Configuration.Middlewares;

namespace Ozon.MerchService.Configuration.StartupFilters;

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