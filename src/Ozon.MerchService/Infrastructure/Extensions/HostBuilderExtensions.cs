using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Ozon.MerchService.Infrastructure.Constants;
using Ozon.MerchService.Infrastructure.ExceptionsFilters;
using Ozon.MerchService.Infrastructure.Interceptors;
using Ozon.MerchService.Infrastructure.OperationFilters;
using Ozon.MerchService.Infrastructure.StartupFilters;

namespace Ozon.MerchService.Infrastructure.Extensions;

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
        return builder
            .AddTracing()
            .AddVersionEndpoint()
            .AddReadyEndpoint()
            .AddLiveEndpoint()
            .AddGrpc()
            .AddStartupFilters()
            .AddGlobalExceptionFilter()
            .AddSwagger();
    }

    private static IHostBuilder AddGrpc(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggingInterceptor>();
                options.Interceptors.Add<ExceptionInterceptor>();
            });
        });

        return builder;
    }

    private static IHostBuilder AddGlobalExceptionFilter(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
        });

        return builder;
    }
    
    private static IHostBuilder AddStartupFilters(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton<IStartupFilter, ResponseLogging>()
                .AddSingleton<IStartupFilter, RequestLogging>();
        });

        return builder;
    }
    
    private static IHostBuilder AddTracing(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton<IStartupFilter, Tracing>();
        });

        return builder;
    }

    private static IHostBuilder AddVersionEndpoint(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, VersionInformation>();
        });

        return builder;
    }

    private static IHostBuilder AddReadyEndpoint(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, ReadyResponse>();
        });

        return builder;
    }

    private static IHostBuilder AddLiveEndpoint(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, LiveResponse>();
        });

        return builder;
    }
    
    internal static IHostBuilder AddSwagger(this IHostBuilder builder)
    {
        // builder.ConfigureServices(services =>
        //         services
        //             .AddSwaggerGen(options =>
        //             {
        //                 options.SwaggerDoc(
        //                     Names.SwaggerDocVersion,
        //                     new OpenApiInfo
        //                     {
        //                         Title = Names.GetApplicationName(),
        //                         Version = Names.SwaggerDocVersion
        //                     });
        //                 options.CustomSchemaIds(selector => selector.FullName);
        //
        //                 var xmlFileName = GetXmlFileName();
        //                 var xmlFilePath = GetXmlFilePath(xmlFileName);
        //             
        //                 options.IncludeXmlComments(xmlFilePath);
        //                 options.OperationFilter<AddSwaggerTestHeader>();
        //             })
        //             .AddSingleton<IStartupFilter, Swagger>());

        return builder;
    }

    private static string GetXmlFileName()
    {
        return Names.GetApplicationName();
    }

    private static string GetXmlFilePath(string xmlFileName)
    {
        return Path.Combine(AppContext.BaseDirectory, $"{xmlFileName}.xml");
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

        kestrelServerOptions.Listen(
            address, 
            port.Value, 
            listenOptions => { listenOptions.Protocols = protocols; });
    }
}