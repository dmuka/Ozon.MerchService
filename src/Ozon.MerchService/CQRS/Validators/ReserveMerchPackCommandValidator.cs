using CSharpCourse.Core.Lib.Enums;
using FluentValidation;
using Ozon.MerchService.CQRS.Commands;

namespace Ozon.MerchService.CQRS.Validators;

/// <summary>
/// Reserve merch pack command validator
/// </summary>
public class ReserveMerchPackCommandValidator : AbstractValidator<ReserveMerchPackCommand>
{
    /// <summary>
    /// Reserve merch pack command validator constructor
    /// </summary>
    public ReserveMerchPackCommandValidator()
    {
        RuleFor(command => command.MerchPackRequest.Employee.Id)
            .NotEmpty().GreaterThan(default(long));

        RuleFor(command => command.MerchPackRequest.Employee.Email.Value)
            .NotEmpty().EmailAddress();

        RuleFor(command => command.MerchPackRequest.HrEmail.Value)
            .NotEmpty().EmailAddress();

        RuleFor(command => command.MerchPackRequest.Employee.FullName.Value)
            .NotEmpty().Length(4,100);

        RuleFor(command => command.EventType)
            .Must(BeValidEventTypeValue);
    }

    private bool BeValidEventTypeValue(EmployeeEventType? eventType)
    {
        return eventType switch
        {
            EmployeeEventType.Hiring or EmployeeEventType.ConferenceAttendance or null => true,
            _ => false
        };
    }
}