using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

public interface IMessageBroker
{
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