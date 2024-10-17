using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.RegisterUser;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class RegisterUserRequestMappers
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request)
    {
        return new RegisterUserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            ClientUri = request.ClientUri
        };
    }
}