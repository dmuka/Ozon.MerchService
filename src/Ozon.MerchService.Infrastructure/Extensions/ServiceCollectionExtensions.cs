using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Configuration;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Implementations;
using Ozon.StockApi.Grpc;

namespace Ozon.MerchService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<KafkaConfiguration>(configuration.GetSection(nameof(KafkaConfiguration)))
            .AddSingleton<IMessageBroker, Infrastructure.MessageBroker.Implementations.MessageBroker>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IMerchPacksRepository, MerchPacksRepository>()
            .AddScoped<IMerchPackRequestRepository, MerchPackRequestsRepository>()
            .AddScoped<IEmployeeRepository, EmployeesRepository>();

        return services;
    }
        
    public static IServiceCollection AddStockGrpcServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionAddress = configuration
            .GetSection(nameof(StockGrpcServiceConfiguration))
            .Get<StockGrpcServiceConfiguration>().ServerAddress;
        
        if (string.IsNullOrWhiteSpace(connectionAddress))
            connectionAddress = configuration.Get<StockGrpcServiceConfiguration>().ServerAddress;

        services.AddScoped<StockApiGrpc.StockApiGrpcClient>(opt =>
        {
            var channel = GrpcChannel.ForAddress(connectionAddress);
            
            return new StockApiGrpc.StockApiGrpcClient(channel);
        });


        return services;
    }
}