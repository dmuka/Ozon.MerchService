using System.Reflection;
using Microsoft.OpenApi.Models;
using Ozon.MerchandizeService.Configuration.Constants;
using Ozon.MerchandizeService.Configuration.Middlewares;
using Ozon.MerchandizeService.Configuration.OperationFilters;
using Ozon.MerchandizeService.Configuration.StartupFilters;

namespace Ozon.MerchandizeService.Configuration.Extensions;

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
            services.AddControllers();

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