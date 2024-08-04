using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Implementations;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Implementations;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add kafka configuration and service
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Set of key-value application configuration properties</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<KafkaConfiguration>(configuration.GetSection(nameof(KafkaConfiguration)))
            .AddSingleton<IMessageBroker, MessageBroker.Implementations.MessageBroker>();

        return services;
    }

    /// <summary>
    /// Add repositories and orm
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services
            .AddTransient<IDapperQuery, DapperQuery>()
            .AddTransient<IMerchPacksRepository, MerchPacksRepository>()
            .AddTransient<IMerchPackRequestRepository, MerchPackRequestsRepository>()
            .AddTransient<IEmployeeRepository, EmployeesRepository>()
            .AddTransient<IRepository, Repository>();

        return services;
    }
        
    /// <summary>
    /// Add stock grpc service client
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Set of key-value application configuration properties</param>
    /// <returns>Collection of service descriptors</returns>
    public static IServiceCollection AddStockGrpcServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionAddress = configuration
            .GetSection(nameof(StockGrpcServiceConfiguration))
            .Get<StockGrpcServiceConfiguration>().ServerAddress;
        
        if (string.IsNullOrWhiteSpace(connectionAddress))
            connectionAddress = configuration.Get<StockGrpcServiceConfiguration>().ServerAddress;

        services.AddTransient<StockApiGrpc.StockApiGrpcClient>(opt =>
        {
            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            });
            
            var channel = GrpcChannel.ForAddress(connectionAddress,
                new GrpcChannelOptions { LoggerFactory = loggerFactory });
            
            return new StockApiGrpc.StockApiGrpcClient(channel);
        });


        return services;
    }
}