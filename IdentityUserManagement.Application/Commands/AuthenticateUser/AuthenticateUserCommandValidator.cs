using FluentValidation;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required");
    }
    
}