using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.User;
using Core.Enums;
using FluentValidation;

namespace Core.Validators.User
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            // RuleFor(x => x.Credential)
            //     .NotEmpty()
            //     .When(x => !string.IsNullOrEmpty(x.Provider))
            //     .WithMessage("Credential is required.");
            RuleFor(x => x)
            .Must(x =>
                (!string.IsNullOrEmpty(x.Credential) && !string.IsNullOrEmpty(x.Provider)) ||
                (string.IsNullOrEmpty(x.Credential) && !string.IsNullOrEmpty(x.Email) && !string.IsNullOrEmpty(x.Password))
            )
            .WithMessage("If Credential is provided, Provider must also be provided. Otherwise, Email and Password are required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Provider)
                .Must(provider => Enum.TryParse<AuthProvider>(provider, true, out _))
                .WithMessage("Invalid provider.");
        }
    }
}   