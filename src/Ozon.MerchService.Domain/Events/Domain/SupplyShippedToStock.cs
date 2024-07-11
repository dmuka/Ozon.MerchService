using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace Ozon.MerchService.Domain.Events.Domain;

public class SupplyShippedToStockEvent(SupplyShippedEvent supplyShippedEvent) : INotification
{
    public SupplyShippedEvent SupplyShippedEvent { get; } = supplyShippedEvent;
}