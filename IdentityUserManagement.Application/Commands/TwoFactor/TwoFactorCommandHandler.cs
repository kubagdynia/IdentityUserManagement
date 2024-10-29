using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.TwoFactor;

internal class TwoFactorCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator)
    : IRequestHandler<TwoFactorCommand, TwoFactorCommandResponse>
{
    public async Task<TwoFactorCommandResponse> Handle(TwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        
        if (user is null)
        {
            return new TwoFactorCommandResponse { ErrorType = BaseDomainErrorType.BadRequest };
        }
        
        var validVerification = await userManager.VerifyTwoFactorTokenAsync(user, request.Provider!, request.Token!);
        
        if (!validVerification)
        {
            return new TwoFactorCommandResponse()
                .AddError("2FactorValidationFailed", "Two-factor validation failed", BaseDomainErrorType.BadRequest);
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenGenerator.GenerateToken(user, roles);
        
        await userManager.ResetAccessFailedCountAsync(user);

        return new TwoFactorCommandResponse { IsAuthSuccessful = true, Token = token };
    }
}