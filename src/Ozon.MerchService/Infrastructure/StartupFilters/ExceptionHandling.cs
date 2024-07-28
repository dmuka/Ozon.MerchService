using Ozon.MerchService.Infrastructure.Middlewares;

namespace Ozon.MerchService.Infrastructure.StartupFilters;

/// <summary>
/// Exception handling startup filter
/// </summary>
internal class ExceptionHandling : IStartupFilter
{
    /// <summary>
    /// Handle exception middleware 
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
            application.UseMiddleware<ExceptionMiddleware>();
            next(application);
        };
    }
}