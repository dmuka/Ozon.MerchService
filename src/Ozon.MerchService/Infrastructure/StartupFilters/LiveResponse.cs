using Ozon.MerchService.Infrastructure.Constants;
using Ozon.MerchService.Infrastructure.Middlewares;

namespace Ozon.MerchService.Infrastructure.StartupFilters;

/// <summary>
/// Live response startup filter
/// </summary>
internal class LiveResponse : IStartupFilter
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