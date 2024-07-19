using System.Reflection;
using Npgsql;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Infrastructure.BackgroundServices;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Ozon.MerchService.Infrastructure.Services.Implementations;
using Ozon.MerchService.Infrastructure.Services.Interfaces;
using Ozon.MerchService.Services.Implementations;
using Ozon.MerchService.Services.Interfaces;
using NpgsqlConnectionFactory = Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations.NpgsqlConnectionFactory;

namespace Ozon.MerchService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddScoped<IQueuedRequestsService, QueuedRequestsService>()
            .AddScoped<IStockGrpcService, StockGrpcService>();

        return services;
    }
        
    public static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DbConnectionOptions>(configuration.GetSection(nameof(DbConnectionOptions)));
            
        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITracker, Tracker>();

        return services;
    }
        
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services
            .AddHostedService<StockReplenishedService>()
            .AddHostedService<EmployeeNotificationService>();

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStockGrpcServiceClient(configuration);
            
        return services;
    }  
}