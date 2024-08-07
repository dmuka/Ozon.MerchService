using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.MerchService.Infrastructure.Configuration.MessageBroker;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Exception = System.Exception;

namespace Ozon.MerchService.Infrastructure.MessageBroker.Implementations;

public class MessageBroker : IMessageBroker
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly ProducerConfig _producerConfig;
    private readonly ILogger<MessageBroker> _logger;
    
    public KafkaConfiguration Configuration { get; }
        
        public MessageBroker(IOptions<KafkaConfiguration> options, ILogger<MessageBroker> logger)
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

            try
            {
                await producer.ProduceAsync(topic, message, token);
            }
            catch (Exception exception)
            {
                throw new BrokerException($"Broker produce exception: {exception.Message}", exception);
            }
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
                    try
                    {
                        await Task.Yield();
                        
                        var consumeResult = consumer.Consume(token);
                        if (consumeResult is null)
                        {
                            _logger.LogWarning("Consume result is null.");
                            continue;
                        }

                        _logger.LogInformation("Consumed message: {Message}", consumeResult.Message.Value);

                        await processMessage(consumeResult.Message.Value, token);

                        consumer.Commit();
                        _logger.LogInformation("Committed offset: {Offset}", consumeResult.Offset);
                    }
                    catch (Exception ex)
                    {
                        if (ex is OperationCanceledException)
                        {
                            _logger.LogInformation("Operation canceled. Exiting consume loop.");
                            break;
                        }

                        if (ex is KafkaException kafkaException)
                        {
                            if (kafkaException.Error.Code == ErrorCode.UnknownTopicOrPart)
                            {
                                _logger.LogError("Consume topic {Topic} error: {Error}. Retrying in 5 seconds...",
                                    topic, kafkaException.Error.Reason);
                                await Task.Delay(TimeSpan.FromSeconds(5), token);
                            }
                            else
                            {
                                _logger.LogError("Kafka error: {Message}. Retrying in 5 seconds...",
                                    kafkaException.Error.Reason);
                                await Task.Delay(TimeSpan.FromSeconds(5), token);
                            }
                        }
                        else
                        {
                            _logger.LogError("Unexpected error: {ex} {Message} Continuing execution.", ex.InnerException, ex.StackTrace);
                            await Task.Delay(TimeSpan.FromSeconds(5), token);
                        }
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }
}