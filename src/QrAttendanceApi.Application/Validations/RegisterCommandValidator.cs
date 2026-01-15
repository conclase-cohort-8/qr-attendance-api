using FluentValidation;
using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.Helpers;

namespace QrAttendanceApi.Application.Validations
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(v => v.FullName).NotEmpty()
                .WithMessage("Name is a required field.");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(v => v).Must(args => ValidatorHelper.IsAValidFullName(args.FullName))
                .WithMessage("Please enter a valid full name");
            RuleFor(v => v).Must(args => ValidatorHelper.IsAValidPhoneNumber(args.PhoneNumber))
                .WithMessage("Please enter a valid phone number");

            RuleFor(v => v).Must(args => ValidatorHelper.IsAValidName(args.FullName))
                .WithMessage("Please enter a valid name.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.");
            RuleFor(v => v)
                .Must(args => ValidatorHelper.PasswordMatch(args.Password, args.ComfirmPassword))
                .WithMessage("Password and Confirm Password must match.");
        } 
    }
}
