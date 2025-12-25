using FluentValidation;
using QrAttendanceApi.Application.Commands.Accounts;
using System.Text.RegularExpressions;

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

            RuleFor(v => v).Must(args => IsAValidFullName(args.FullName))
                .WithMessage("Please enter a valid full name");
            RuleFor(v => v).Must(args => IsAValidPhoneNumber(args.PhoneNumber))
                .WithMessage("Please enter a valid phone number");

            RuleFor(v => v).Must(args => IsAValidName(args.FullName))
                .WithMessage("Please enter a valid name.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.");
            RuleFor(v => v)
                .Must(args => PasswordMatch(args.Password, args.ComfirmPassword))
                .WithMessage("Password and Confirm Password must match.");
        }

        private bool IsAValidName(string name)
        {
            var nameCount = name.Split(' ').Length;
            if(nameCount <= 1 && nameCount > 3)
            {
                return false;
            }

            var match = Regex.Match(name, @"^[A-Za-z]+(?: [A-Za-z]+)*$");
            return match.Success;
        }

        private bool PasswordMatch(string password, string comparePassword)
        {
            return !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(comparePassword)
                && password.Equals(comparePassword);
        }

        private bool IsAValidFullName(string fullName)
        {
            var splitted = fullName.Split(' ');
            return splitted.Length > 1;
        }

        private bool IsAValidPhoneNumber(string? phoneNumber)
        {
            if(string.IsNullOrWhiteSpace(phoneNumber)) return true;

            var match = Regex.Match(phoneNumber, @"^\+(\d{1,4})[-\s]?(\d{6,14})$");

            return match.Success;
        } 
    }
}
