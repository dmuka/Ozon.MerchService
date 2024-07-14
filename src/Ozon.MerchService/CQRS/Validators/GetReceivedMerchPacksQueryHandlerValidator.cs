using FluentValidation;
using Ozon.MerchService.CQRS.Queries;

namespace Ozon.MerchService.CQRS.Validators;

public class GetReceivedMerchPacksQueryHandlerValidator : AbstractValidator<GetReceivedMerchPacksQuery>
{
    
    public GetReceivedMerchPacksQueryHandlerValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty().GreaterThan(default(long));
    }
}