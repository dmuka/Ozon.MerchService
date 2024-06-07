using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ozon.MerchandizeService;

public class Startup(IConfiguration configuration)
{
    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
        application.UseRouting();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", () => "Bla bla bla");
        });
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        
    }
}