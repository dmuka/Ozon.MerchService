using FluentValidation;
using Ozon.MerchService.CQRS.Queries;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Get received merch packs query handler validator
/// </summary>
public class GetReceivedMerchPacksQueryValidator : AbstractValidator<GetReceivedMerchPacksQuery>
{
    /// <summary>
    /// Get received merch packs query handler validator constructor
    /// </summary>
    public GetReceivedMerchPacksQueryValidator()
    {
        RuleFor(query => query.EmployeeId)
            .NotEmpty().GreaterThan(default(long));
    }
}