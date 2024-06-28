using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Ozon.MerchService.Infrastructure.Configuration.StartupFilters;

/// <summary>
/// Swagger startup filter
/// </summary>
public class Swagger : IStartupFilter
{
    /// <summary>
    /// Add Swagger and SwaggerUI middlewares
    /// </summary>
    /// <param name="next">The Configure method to extend.</param>
    /// <returns>A modified <see cref="T:System.Action" />.</returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return application =>
        {
            application
                .UseSwagger()
                .UseSwaggerUI();

            next(application);
        };
    }
}