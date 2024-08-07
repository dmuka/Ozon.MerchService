using System.Text.Json;
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
    //IMediator mediator,
    IMessageBroker broker) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = broker.Configuration.EmployeeNotificationEventTopic;
        
        await broker.ConsumeAsync(topic, scopeFactory, PublishEvent, stoppingToken);
    }

    private async Task PublishEvent(string serializedMessage, CancellationToken token)
    {
        var message = JsonSerializer.Deserialize<NotificationEvent>(serializedMessage);
        
        if (message is null) return;

        var @event = new EmployeeNeededMerchEvent(message);

        var mediator = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMediator>();
        
        await mediator.Publish(@event, token);
    }
}