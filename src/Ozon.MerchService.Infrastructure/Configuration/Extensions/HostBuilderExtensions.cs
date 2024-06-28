using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ozon.MerchService.Infrastructure.Configuration.ExceptionsFilters;

namespace Ozon.MerchService.Infrastructure.Configuration.Extensions;

/// <summary>
/// Contain host builder extensions
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Add infrastructure services in IOC container
    /// </summary>
    /// <param name="builder">Host builder object</param>
    /// <returns></returns>
    public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddSwagger()
                .AddStartupFilters()
                .AddControllers(options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                });
        });

        return builder;
    }

    private static IHostBuilder AddPorts(this IHostBuilder builder)
    {
        
        var httpPortEnv = Environment.GetEnvironmentVariable("HTTP_PORT");
        
        if (!int.TryParse(httpPortEnv, out var httpPort))
        {
            httpPort = 5080;
        };
        
        var grpcPortEnv = Environment.GetEnvironmentVariable("GRPC_PORT");
        
        if (!int.TryParse(grpcPortEnv, out var grpcPort))
        {
            grpcPort = 5082;
        };
        
        builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.ConfigureKestrel(
                options =>
                {
                    Listen(options, httpPort, HttpProtocols.Http1);
                    Listen(options, grpcPort, HttpProtocols.Http2);
                });
        });

        return builder;
    }
        
    private static void Listen(
        KestrelServerOptions kestrelServerOptions, 
        int? port, 
        HttpProtocols protocols)
    {
        if (port == null) return;

        var address = IPAddress.Any;

        kestrelServerOptions.Listen(address, port.Value, listenOptions => { listenOptions.Protocols = protocols; });
    }
}