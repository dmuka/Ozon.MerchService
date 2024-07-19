using Ozon.MerchService.Infrastructure.Constants;
using Ozon.MerchService.Infrastructure.Middlewares;

namespace Ozon.MerchService.Infrastructure.StartupFilters;

/// <summary>
/// Application version startup filter
/// </summary>
internal class VersionInformation : IStartupFilter
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