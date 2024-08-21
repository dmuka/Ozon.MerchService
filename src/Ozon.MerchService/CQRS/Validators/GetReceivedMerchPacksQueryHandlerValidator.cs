using FluentValidation;
using Ozon.MerchService.CQRS.Queries;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Get received merch packs query handler validator
/// </summary>
public class GetReceivedMerchPacksQueryHandlerValidator : AbstractValidator<GetReceivedMerchPacksQuery>
{
    
    /// <summary>
    /// Get received merch packs query handler validator constructor
    /// </summary>
    public GetReceivedMerchPacksQueryHandlerValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty().GreaterThan(default(long));
    }
}