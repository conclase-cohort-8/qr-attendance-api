using FluentValidation;
using QrAttendanceApi.Application.Commands.Accounts;

namespace QrAttendanceApi.Application.Validations
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(v => v.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Minimum password length is 8 characters");
        }
    }
}
