using Ozon.MerchService.Services.Interfaces;

namespace Ozon.MerchService;

/// <summary>
/// Configure and setup application
/// </summary>
/// <param name="configuration">Configuration object</param>
public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// Add domain services in IOC container
    /// </summary>
    /// <param name="services">IOC container</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMerchService, Services.Implementations.MerchService>();
    }
    
    /// <summary>
    /// Set up the request processing pipeline
    /// </summary>
    /// <param name="application">Application builder object</param>
    /// <param name="environment">Web host environment object</param>
    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
        application.UseRouting();
        
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", () => "Bla bla bla");
            endpoints.MapControllers();
        });
    }
}