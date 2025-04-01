using Core.Dtos.User;
using FluentValidation;

namespace Core.Validators.User
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x)
            .Must(x =>
                (!string.IsNullOrEmpty(x.Credential) && !string.IsNullOrEmpty(x.Provider)) ||
                (string.IsNullOrEmpty(x.Credential) && !string.IsNullOrEmpty(x.Email) && !string.IsNullOrEmpty(x.Password) && !string.IsNullOrEmpty(x.ConfirmPassword))
            )
            .WithMessage("If Credential is provided, Provider must also be provided. Otherwise, Email and Password are required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Credential) && x.ConfirmPassword.Equals(x.Password))
            .WithMessage("Passwords do not match.");
        }
    }
}