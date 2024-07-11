using System.Diagnostics;
using CSharpCourse.Core.Lib.Enums;
using FluentValidation;
using Ozon.MerchService.CQRS.Commands;

namespace Ozon.MerchService.CQRS.Validators;

public class ReserveMerchPackCommandValidator : AbstractValidator<ReserveMerchPackCommand>
{
    public ReserveMerchPackCommandValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty().GreaterThan(default(long));

        RuleFor(command => command.EmployeeEmail)
            .NotEmpty().EmailAddress();

        RuleFor(command => command.HrEmail)
            .NotEmpty().EmailAddress();

        RuleFor(command => command.EmployeeFirstName)
            .NotEmpty().Length(2,50);

        RuleFor(command => command.EmployeeLastName)
            .NotEmpty().Length(2,50);

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