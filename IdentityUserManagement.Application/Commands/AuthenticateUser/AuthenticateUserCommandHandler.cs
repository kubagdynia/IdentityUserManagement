using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

internal class AuthenticateUserCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator)
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

        if (!await userManager.CheckPasswordAsync(user, request.Password!))
        {
            return new AuthenticateUserCommandResponse { ErrorType = BaseDomainErrorType.Unauthorized };
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);

        return new AuthenticateUserCommandResponse { IsAuthSuccessful = true, Token = token };
    }
}