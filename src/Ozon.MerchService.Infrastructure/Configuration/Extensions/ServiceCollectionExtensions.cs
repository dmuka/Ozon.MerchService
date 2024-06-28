using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Ozon.MerchService.Configuration.Constants;
using Ozon.MerchService.Infrastructure.Configuration.OperationFilters;
using Ozon.MerchService.Infrastructure.Configuration.StartupFilters;
using Ozon.MerchService.Infrastructure.Repositories.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Postgres;

namespace Ozon.MerchService.Infrastructure.Configuration.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();

        return services;
    }
    
    internal static IServiceCollection AddStartupFilters(this IServiceCollection services)
    {
        services
            .AddSingleton<IStartupFilter, Swagger>()
            .AddSingleton<IStartupFilter, ResponseLogging>()
            .AddSingleton<IStartupFilter, RequestLogging>()
            .AddSingleton<IStartupFilter, VersionInformation>()
            .AddSingleton<IStartupFilter, LiveResponse>()
            .AddSingleton<IStartupFilter, ReadyResponse>();

        return services;
    }
    
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddSwaggerGen(options =>
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

        return services;
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