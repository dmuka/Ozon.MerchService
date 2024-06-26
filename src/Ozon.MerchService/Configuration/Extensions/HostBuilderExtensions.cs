using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Ozon.MerchService.Configuration.Constants;
using Ozon.MerchService.Configuration.ExceptionsFilters;
using Ozon.MerchService.Configuration.OperationFilters;
using Ozon.MerchService.Configuration.StartupFilters;

namespace Ozon.MerchService.Configuration.Extensions;

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
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            AddStartupFilters(services);
                
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    Names.SwaggerDocVersion,
                    new OpenApiInfo
                    {
                        Title = Names.GetApplicationName(),
                        Version = Names.SwaggerDocVersion
                    });
                options.CustomSchemaIds(selector => selector.FullName);

                var xmlFileName = GetXmlFileName();
                var xmlFilePath = GetXmlFilePath(xmlFileName);
                    
                options.IncludeXmlComments(xmlFilePath);
                options.OperationFilter<AddSwaggerTestHeader>();
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

    private static void AddStartupFilters(IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, Swagger>();
        services.AddSingleton<IStartupFilter, ResponseLogging>();
        services.AddSingleton<IStartupFilter, RequestLogging>();
        services.AddSingleton<IStartupFilter, VersionInformation>();
        services.AddSingleton<IStartupFilter, LiveResponse>();
        services.AddSingleton<IStartupFilter, ReadyResponse>();
    }

    private static string GetXmlFileName()
    {
        return Names.GetApplicationName();
    }

    private static string GetXmlFilePath(string xmlFileName)
    {
        return Path.Combine(AppContext.BaseDirectory, $"{xmlFileName}.xml");
    }
}