using FluentValidation;
using CMCS.Models;


public class ClaimValidator : AbstractValidator<Claim>
{
    public ClaimValidator()
    {
        RuleFor(c => c.HoursWorked).InclusiveBetween(0, 160).WithMessage("Hours worked must be between 0 and 160.");
        RuleFor(c => c.HourlyRate).GreaterThan(0).WithMessage("Hourly rate must be greater than 0.");
        RuleFor(c => c.StartDate).LessThanOrEqualTo(c => c.EndDate).WithMessage("Start date must be earlier than or equal to the end date.");
        RuleFor(c => c.TotalPayment).GreaterThan(0).WithMessage("Total payment must be greater than 0.");
    }
}
