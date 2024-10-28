using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Application.Commands.TwoFactor;

namespace IdentityUserManagement.Api.Endpoints.Mappers;

public static class TwoFactorMappers
{
    public static TwoFactorCommand ToCommand(this TwoFactorRequest request)
    {
        return new TwoFactorCommand
        {
            Email = request.Email,
            Provider = request.Provider,
            Token = request.Token
        };
    }
    
    public static TwoFactorResponse ToResponse(this TwoFactorCommandResponse commandResponse)
    {
        return new TwoFactorResponse(commandResponse.IsAuthSuccessful, commandResponse.Token);
    }
}
