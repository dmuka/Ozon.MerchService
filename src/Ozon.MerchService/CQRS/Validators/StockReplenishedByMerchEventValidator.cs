using FluentValidation;
using Ozon.MerchService.Domain.Events.Domain;

namespace Ozon.MerchService.CQRS.Validators;

public class StockReplenishedByMerchEventValidator : AbstractValidator<StockReplenishedByMerchEvent>
{
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