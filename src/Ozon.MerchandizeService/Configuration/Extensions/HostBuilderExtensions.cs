using System.Reflection;
using Microsoft.OpenApi.Models;
using Ozon.MerchandizeService.Configuration.Constants;
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
                
            services.AddSingleton<IStartupFilter, Swagger>()
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(
                        Names.SwaggerDocVersion,
                        new OpenApiInfo
                        {
                            Title = Names.DefaultApplicationName,
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

    private static string GetXmlFileName()
    {
        return Assembly.GetExecutingAssembly().GetName().Name ?? Names.DefaultApplicationName;
    }

    private static string GetXmlFilePath(string xmlFileName)
    {
        return Path.Combine(AppContext.BaseDirectory, $"{xmlFileName}.xml");
    }
}