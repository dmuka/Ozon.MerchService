using Ozon.MerchService.Infrastructure.Extensions;
using Serilog;

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
        services
            .AddDbConnection(configuration)
            .AddRepositories()
            .AddHostedServices()
            .AddAppServices()
            .AddExternalServices(configuration)
            .AddKafkaServices(configuration)
            .AddTelemetry(configuration)
            .AddSwaggerGen();
        foreach (var service in services)
        {
            Console.WriteLine($"Service: {service.ServiceType.Name}, Lifetime: {service.Lifetime}");
        }
    }
    
    /// <summary>
    /// Set up the request processing pipeline
    /// </summary>
    /// <param name="application">Application builder object</param>
    /// <param name="environment">Web host environment object</param>
    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
        }
        
        application.UseRouting();
        
        application.UseSerilogRequestLogging();

        application.UseSwagger().UseSwaggerUI();
        
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", () => "Bla bla bla");
            endpoints.MapControllers();
        });
    }
}