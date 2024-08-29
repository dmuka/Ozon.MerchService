using FluentValidation;
using Ozon.MerchService.CQRS.Queries;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Get merch pack by id query handler validator
/// </summary>
public class GetMerchPackByIdQueryValidator : AbstractValidator<GetMerchPackByIdQuery>
{
    /// <summary>
    /// Get merch pack by id query handler validator constructor
    /// </summary>
    public GetMerchPackByIdQueryValidator()
    {
        RuleFor(query => query.MerchPackId)
            .NotEmpty().GreaterThan(default(int));
    }
}