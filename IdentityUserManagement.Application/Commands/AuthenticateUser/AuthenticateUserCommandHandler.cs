using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandler(UserManager<User> userManager, IJwtHandler jwtHandler)
    : IRequestHandler<AuthenticateUserCommand, AuthenticateUserCommandResponse>
{
    public async Task<AuthenticateUserCommandResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Email!);
        
        if (user == null)
        {
            return new AuthenticateUserCommandResponse
                { ErrorType = BaseDomainErrorType.BadRequest };
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password!))
        {
            return new AuthenticateUserCommandResponse
                { ErrorType = BaseDomainErrorType.Unauthorized };
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var token = jwtHandler.GenerateToken(user, roles);

        return new AuthenticateUserCommandResponse { IsAuthSuccessful = true, Token = token };
    }
}