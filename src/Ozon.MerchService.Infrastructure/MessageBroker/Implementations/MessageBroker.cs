using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

namespace Ozon.MerchService.Infrastructure.MessageBroker.Implementations;

public class MessageBroker : IMessageBroker
{
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<MessageBroker> _logger;
        
        public KafkaConfiguration Configuration { get; }
        
        private MessageBroker(IOptions<KafkaConfiguration> options, ILogger<MessageBroker> logger)
        {
            Configuration = options.Value;
            _logger = logger;
            
            _consumerConfig = new ConsumerConfig
            {
                GroupId = Configuration.GroupId,
                BootstrapServers = Configuration.BootstrapServers
            };
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = Configuration.BootstrapServers
            };
        }

        public async Task ProduceAsync(string topic, string key, object value, CancellationToken token)
        {
            
            
            var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
            
            var message = new Message<string, string>
            {
                Key = key,
                Value = JsonSerializer.Serialize(value)
            };
            
            await producer.ProduceAsync(topic, message, token);
        }
        
        public async Task ConsumeAsync(string topic, 
            IServiceScopeFactory scopeFactory, 
            Func<string, CancellationToken, Task> processMessage,
            CancellationToken token)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe(topic);
            try
            {
                while (!token.IsCancellationRequested)
                {
                    using var scope = scopeFactory.CreateScope();
                    try
                    {
                        await Task.Yield();
                        var consumeResult = consumer.Consume(token);
                        if (consumeResult is null)
                            continue;

                        await processMessage(consumeResult.Message.Value, token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Consume topic {name} error: {error}", topic, ex.Message);
                    }
                }
            }
            finally
            {
                consumer.Commit();
                consumer.Close();
            }
        }

}