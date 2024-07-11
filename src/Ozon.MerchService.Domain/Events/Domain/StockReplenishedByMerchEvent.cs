using CSharpCourse.Core.Lib.Events;
using CSharpCourse.Core.Lib.Models;
using MediatR;

namespace Ozon.MerchService.Domain.Events.Domain;

public class StockReplenishedByMerchEvent(StockReplenishedEvent stockReplenishedByMerchEvent) : INotification
{
    public IReadOnlyCollection<StockReplenishedItem> Items { get; } = stockReplenishedByMerchEvent.Type;
}