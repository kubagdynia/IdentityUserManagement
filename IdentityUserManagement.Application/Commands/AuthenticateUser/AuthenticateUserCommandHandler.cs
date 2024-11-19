using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Constants;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

internal class AuthenticateUserCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator, IEmailService emailService)
    : IRequestHandler<AuthenticateUserCommand, AuthenticateUserCommandResponse>
{
    public async Task<AuthenticateUserCommandResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Email!);
        
        if (user is null)
        {
            return new AuthenticateUserCommandResponse { ErrorType = BaseDomainErrorType.BadRequest };
        }
        
        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            return new AuthenticateUserCommandResponse()
                .AddError("EmailIsNotConfirmed", "Email is not confirmed", BaseDomainErrorType.Conflict);
        }
        
        if (await userManager.IsLockedOutAsync(user))
        {
            return new AuthenticateUserCommandResponse()
                .AddError("UserAccountIsLockedOut", "User account is locked out", BaseDomainErrorType.Conflict);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password!))
        {
            // Increment the access failed count
            await userManager.AccessFailedAsync(user);
            
            if (await userManager.IsLockedOutAsync(user))
            {
                var content =
                    "Your account is locked out. If you want to reset the password, you can use the Forgot Password link on the Login page.";
                var emailMetadata = new EmailMetadata(user.Email!, "Account locked out", content);

                // Send the email
                await emailService.SendAsync(emailMetadata);

                return new AuthenticateUserCommandResponse()
                    .AddError("UserAccountIsLockedOut", "User account is locked out", BaseDomainErrorType.Conflict);
            }

            return new AuthenticateUserCommandResponse { ErrorType = BaseDomainErrorType.Unauthorized };
        }
        
        if (await userManager.GetTwoFactorEnabledAsync(user))
        {
            return await GenerateTwoFactorToken(user);
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);
        
        await userManager.ResetAccessFailedCountAsync(user);

        return new AuthenticateUserCommandResponse { IsAuthSuccessful = true, Token = token };
    }
    
    private async Task<AuthenticateUserCommandResponse> GenerateTwoFactorToken(User user)
    {
        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
        if (!providers.Contains("Email"))
        {
            return new AuthenticateUserCommandResponse()
                .AddError("Invalid2FactorProvider", "Invalid 2-Factor Provider", BaseDomainErrorType.Conflict);
        }
        
        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        
        EmailTemplateData emailTemplateData = new()
        {
            EmailUser = new EmailUser(Name: user.Email!, Email: user.Email!, FirstName: user.FirstName, LastName: user.LastName),
            ToAddress = user.Email!,
            ActionCode = token
        };
        
        // Send the email
        await emailService.SendAsync(emailTemplateData, EmailTemplateType.TwoFactorAuthentication);
        
        return new AuthenticateUserCommandResponse { RequiresTwoFactor = true, Provider = "Email" };
    }
}