using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.ForgotPassword;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class ForgotPasswordMappers
{
    public static ForgotPasswordCommand ToCommand(this ForgotPasswordRequest request)
    {
        return new ForgotPasswordCommand
        {
            Email = request.Email,
            ClientUri = request.ClientUri
        };
    }
}