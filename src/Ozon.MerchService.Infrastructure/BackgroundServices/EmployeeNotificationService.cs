using System.Text.Json;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ozon.MerchService.Domain.Events.Domain;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;

namespace Ozon.MerchService.Infrastructure.BackgroundServices;

public class EmployeeNotificationService(
    IMessageBroker broker,
    IServiceScopeFactory scopeFactory,
    IMediator mediator) : BackgroundService
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
        
        await mediator.Publish(new EmployeeNeededMerchEvent(message), token);
    }
}