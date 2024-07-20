using Ozon.MerchService.Infrastructure.Middlewares;

namespace Ozon.MerchService.Infrastructure.StartupFilters;

internal class Tracing : IStartupFilter
{
    /// <summary>
    /// Add tracing middleware
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
        
            application.UseMiddleware<TracingMiddleware>();
            
            next(application);
        };
    }
}