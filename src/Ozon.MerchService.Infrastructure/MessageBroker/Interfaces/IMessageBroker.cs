using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;

namespace Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

public interface IMessageBroker
{
    KafkaConfiguration Configuration { get; }
    /// <summary>
    /// Produce message to topic
    /// </summary>
    Task ProduceAsync(string topic, string key, object value, CancellationToken token);

    /// <summary>
    /// Consume messages from topic
    /// </summary>
    Task ConsumeAsync(string topic, IServiceScopeFactory scopeFactory,
        Func<string, CancellationToken, Task> processMessage, CancellationToken token);
}