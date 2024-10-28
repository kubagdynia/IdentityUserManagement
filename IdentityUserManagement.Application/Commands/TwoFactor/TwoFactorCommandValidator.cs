using FluentValidation;

namespace IdentityUserManagement.Application.Commands.TwoFactor;

public class TwoFactorCommandValidator : AbstractValidator<TwoFactorCommand>
{
    public TwoFactorCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Provider)
            .NotEmpty().WithMessage("Provider is required");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}