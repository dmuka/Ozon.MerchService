namespace Ozon.MerchService.Infrastructure.StartupFilters;

/// <summary>
/// Swagger startup filter
/// </summary>
internal class Swagger : IStartupFilter
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