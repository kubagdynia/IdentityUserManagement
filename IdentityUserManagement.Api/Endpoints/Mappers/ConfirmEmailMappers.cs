using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.ConfirmEmail;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class ConfirmEmailMappers
{
    public static ConfirmEmailCommand ToCommand(this ConfirmEmailRequest request)
    {
        return new ConfirmEmailCommand
        {
            Email = request.Email,
            Token = request.Token
        };
    }
}