using FluentValidation;
using IdentityUserManagement.Application.Dto;

namespace IdentityUserManagement.Application.Validators;

public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long");
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }

}