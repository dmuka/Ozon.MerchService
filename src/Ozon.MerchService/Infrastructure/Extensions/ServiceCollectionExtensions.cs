using MediatR;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ozon.MerchService.CQRS.Handlers.Events;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.BackgroundServices;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Constants;
using Ozon.MerchService.Infrastructure.Mediatr;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Ozon.MerchService.Infrastructure.Services.Implementations;
using Ozon.MerchService.Infrastructure.Services.Interfaces;
using Ozon.MerchService.Mapper;
using Ozon.MerchService.Services.Implementations;
using Ozon.MerchService.Services.Interfaces;
using NpgsqlConnectionFactory = Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations.NpgsqlConnectionFactory;

namespace Ozon.MerchService.Infrastructure.Extensions;

/// <summary>
/// Service collection extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services
            .AddMediatR(mediatrServiceConfiguration =>
            {
                mediatrServiceConfiguration.RegisterServicesFromAssemblyContaining(typeof(Program));

                mediatrServiceConfiguration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                mediatrServiceConfiguration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            })
            .AddAutoMapper(typeof(MerchServiceMapperProfile).Assembly)
            //.AddScoped(typeof(IPipelineBehavior<,>), typeof(ScopedBehavior<,>))
            .AddScoped<IQueuedRequestsService, QueuedRequestsService>()
            .AddScoped<IStockGrpcService, StockGrpcService>();

        return services;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Set of key-value application configuration properties</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(Names.GetApplicationName()))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                //.AddConsoleExporter()
                .AddSource("GetReceivedMerchPacksQueryHandler")
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = configuration["Jaeger:Host"];
                    options.AgentPort = configuration.GetValue<int>("Jaeger:Port");
                }))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation());
                //.AddConsoleExporter());

        return services;
    }    
        
    /// <summary>
    /// Add db connection services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Set of key-value application configuration properties</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbConnectionOptions>(configuration.GetSection(nameof(DbConnectionOptions)));
            
        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITracker, Tracker>();

        return services;
    }
        
    /// <summary>
    /// Add hosted services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services
            .AddHostedService<StockReplenishedService>()
            .AddHostedService<EmployeeNotificationService>();

        return services;
    }

    /// <summary>
    /// Add external services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Set of key-value application configuration properties</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddExternalServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStockGrpcServiceClient(configuration);
            
        return services;
    }  
}