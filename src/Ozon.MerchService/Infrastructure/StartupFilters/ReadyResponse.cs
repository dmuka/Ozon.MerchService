using Ozon.MerchService.Infrastructure.Constants;
using Ozon.MerchService.Infrastructure.Middlewares;

namespace Ozon.MerchService.Infrastructure.StartupFilters;

/// <summary>
/// Ready response startup filter
/// </summary>
internal class ReadyResponse : IStartupFilter
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