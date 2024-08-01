using System.Text.Json;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

namespace Ozon.MerchService.Infrastructure.BackgroundServices;

public class StockReplenishedService(
    IServiceScopeFactory scopeFactory,
    ILogger<StockReplenishedService> logger,
    IMediator mediator,
    IMessageBroker broker) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = broker.Configuration.StockReplenishedEventTopic;

        while (!stoppingToken.IsCancellationRequested)
        {
            await broker.ConsumeAsync(topic, scopeFactory, PublishEvent, stoppingToken);
        }
    }

    private async Task PublishEvent(string serializedMessage, CancellationToken token)
    {
        var message = JsonSerializer.Deserialize<StockReplenishedEvent>(serializedMessage);
        
        if (message is null) return;
        
        await mediator.Publish(new StockReplenishedByMerchEvent(message), token);
    }
}