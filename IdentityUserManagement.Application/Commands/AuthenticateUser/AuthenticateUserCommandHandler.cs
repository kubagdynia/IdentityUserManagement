using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Application.Security;
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
            var response = new AuthenticateUserCommandResponse();
            response.AddError("EmailIsNotConfirmed", "Email is not confirmed", BaseDomainErrorType.Conflict);
            return response;
        }
        
        if (await userManager.IsLockedOutAsync(user))
        {
            var response = new AuthenticateUserCommandResponse();
            response.AddError("UserAccountIsLockedOut", "User account is locked out", BaseDomainErrorType.Conflict);
            return response;
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
                
                var response = new AuthenticateUserCommandResponse();
                response.AddError("UserAccountIsLockedOut", "User account is locked out", BaseDomainErrorType.Conflict);
                return response;
            }

            return new AuthenticateUserCommandResponse { ErrorType = BaseDomainErrorType.Unauthorized };
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);
        
        await userManager.ResetAccessFailedCountAsync(user);

        return new AuthenticateUserCommandResponse { IsAuthSuccessful = true, Token = token };
    }
}