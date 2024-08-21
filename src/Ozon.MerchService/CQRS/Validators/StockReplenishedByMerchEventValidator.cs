using FluentValidation;
using Ozon.MerchService.Domain.Events.Domain;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Stock replenished by merch event validator
/// </summary>
public class StockReplenishedByMerchEventValidator : AbstractValidator<StockReplenishedByMerchEvent>
{
    /// <summary>
    /// Stock replenished by merch event validator constructor
    /// </summary>
    public StockReplenishedByMerchEventValidator()
    {
        RuleForEach(@event => @event.Items)
            .ChildRules(item => 
            {
                item.RuleFor(x => x.Sku)
                    .GreaterThan(default(long));
            });
    }
}