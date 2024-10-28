using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.AuthenticateUser;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class AuthenticateUserMappers
{
    public static AuthenticateUserCommand ToCommand(this AuthenticateUserRequest request)
    {
        return new AuthenticateUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };
    }
    
    public static AuthenticateUserResponse ToResponse(this AuthenticateUserCommandResponse commandResponse)
    {
        return new AuthenticateUserResponse(commandResponse.IsAuthSuccessful, commandResponse.Token,
            commandResponse.RequiresTwoFactor, commandResponse.Provider);
    }
}