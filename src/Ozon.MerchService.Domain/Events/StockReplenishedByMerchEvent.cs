using CSharpCourse.Core.Lib.Models;
using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace Ozon.MerchService.Domain.Events;

public class StockReplenishedByMerchEvent(StockReplenishedEvent stockReplenishedByMerchEvent) : INotification
{
    public IReadOnlyCollection<StockReplenishedItem> Items { get; } = stockReplenishedByMerchEvent.Type;
}