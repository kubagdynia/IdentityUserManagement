using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.ResetPassword;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class ResetPasswordMappers
{
    public static ResetPasswordCommand ToCommand(this ResetPasswordRequest request)
    {
        return new ResetPasswordCommand
        {
            Email = request.Email,
            Token = request.Token,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };
    }
}