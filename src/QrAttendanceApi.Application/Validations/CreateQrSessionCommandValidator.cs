using FluentValidation;
using QrAttendanceApi.Application.Commands.QRs;

namespace QrAttendanceApi.Application.Validations
{
    internal class CreateQrSessionCommandValidator : AbstractValidator<CreateQrSessionCommand>
    {
        public CreateQrSessionCommandValidator()
        {
            RuleFor(s => s.DurationMinutes)
                .GreaterThan(0).WithMessage("Session duration must be greater than 0 minute");
            RuleFor(s => s.LateAfterMinutes)
                .GreaterThan(0).WithMessage("Late threshold minutes must be greater than 0.");
            RuleFor(s => s.Type)
                .IsInEnum().WithMessage("Please select a valid session type");
            RuleFor(s => s.Description)
                .NotEmpty().WithMessage("Session description is a required field.");
            RuleFor(s => s)
                .Must(args => VaidDate(args.StartTime));
        }

        private bool VaidDate(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
