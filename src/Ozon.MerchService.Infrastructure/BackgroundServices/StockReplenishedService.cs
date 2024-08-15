using System.Text.Json;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

namespace Ozon.MerchService.Infrastructure.BackgroundServices;

public class StockReplenishedService(
    IServiceScopeFactory scopeFactory,
    ILogger<StockReplenishedService> logger,
    IMessageBroker broker) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = broker.Configuration.StockReplenishedEventTopic;
        
        if (topic is null) throw new ArgumentException("Stock replenished event topic value is null.");
        
        logger.LogInformation("Start consume topic: {topic}.", topic);
        
        await broker.ConsumeAsync(topic, scopeFactory, PublishEvent, stoppingToken);
        
        logger.LogInformation("End consume topic: {topic}.", topic);
    }

    private async Task PublishEvent(string serializedMessage, CancellationToken token)
    {
        var message = JsonSerializer.Deserialize<StockReplenishedEvent>(serializedMessage);
        
        if (message is null) throw new ArgumentException("Stock replenished event message value is null.");;
        
        logger.LogInformation("Start publish event: {event}.", nameof(StockReplenishedEvent));

        var mediator = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMediator>();
        
        await mediator.Publish(message, token);
        
        logger.LogInformation("End publish event: {event}.", nameof(StockReplenishedEvent));
    }
}