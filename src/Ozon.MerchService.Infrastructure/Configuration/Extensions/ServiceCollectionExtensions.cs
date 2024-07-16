using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Ozon.MerchService.Configuration.Constants;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.BackgroundServices;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.Configuration.OperationFilters;
using Ozon.MerchService.Infrastructure.Configuration.StartupFilters;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Implementations;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Postgres;

namespace Ozon.MerchService.Infrastructure.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddRepositories()
            .AddHostedService<StockReplenishedService>()
            .AddHostedService<EmployeeNotificationService>();

        return services;
    }
        
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaConfiguration>(configuration);
        services.AddSingleton<IMessageBroker, Infrastructure.MessageBroker.Implementations.MessageBroker>();

        return services;
    }
    
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IMerchPacksRepository, MerchPacksRepository>()
            .AddScoped<IMerchPackRequestRepository, MerchPackRequestsRepository>()
            .AddScoped<IEmployeeRepository, EmployeesRepository>();

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