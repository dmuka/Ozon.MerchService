using System.Text.Json;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

namespace Ozon.MerchService.Infrastructure.BackgroundServices;

public class EmployeeNotificationService(
    IServiceScopeFactory scopeFactory,
    ILogger<StockReplenishedService> logger,
    IMediator mediator,
    IMessageBroker broker) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = broker.Configuration.EmployeeNotificationEventTopic;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                {
                    await broker.ConsumeAsync(topic, scopeFactory, PublishEvent, stoppingToken);
                }
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error consuming topic {Topic}", topic);

                if (ex is KafkaException kafkaException &&
                    kafkaException.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    logger.LogError("Topic {Topic} is not available. Retrying in 5 seconds...", topic);

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                else
                {
                    logger.LogError("Unexpected error: {Message}. Stopping execution.", ex.Message);
                    
                    break; // or continue, based on current error handling policy
                }
            }
        }
    }

    private async Task PublishEvent(string serializedMessage, CancellationToken token)
    {
        var message = JsonSerializer.Deserialize<NotificationEvent>(serializedMessage);
        
        if (message is null) return;
        
        await mediator.Publish(new EmployeeNeededMerchEvent(message), token);
    }
}