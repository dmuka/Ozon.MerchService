using FluentValidation;
using Ozon.MerchService.CQRS.Commands;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Create merch pack command handler validator
/// </summary>
public class CreateMerchPackCommandValidator : AbstractValidator<CreateMerchPackCommand>
{
    /// <summary>
    /// Create merch pack command handler validator constructor
    /// </summary>
    public CreateMerchPackCommandValidator()
    {
        RuleFor(command => command.MerchPackName)
            .NotEmpty();
        RuleFor(command => command.MerchItems)
            .NotEmpty();
        RuleForEach(command => command.MerchItems)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ItemTypeId).GreaterThan(default(int));
                item.RuleFor(i => i.ItemTypeName).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(default(int));
            });
    }
}