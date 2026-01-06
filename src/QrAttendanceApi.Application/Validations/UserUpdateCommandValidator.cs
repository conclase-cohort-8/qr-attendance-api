using FluentValidation;
using QrAttendanceApi.Application.Commands.Users;
using QrAttendanceApi.Application.Helpers;

namespace QrAttendanceApi.Application.Validations
{
    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name field is required");
            RuleFor(x => x)
                .Must(args => ValidatorHelper.IsAValidName(args.FullName))
                .WithMessage("Please enter a valid full name");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number field is required");
            RuleFor(x => x)
                .Must(a => ValidatorHelper.IsAValidPhoneNumber(a.PhoneNumber))
                .WithMessage("Phone number must be in correct format");
        }
    }
}
