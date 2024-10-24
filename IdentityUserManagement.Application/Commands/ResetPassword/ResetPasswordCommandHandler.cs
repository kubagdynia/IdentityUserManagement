using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Extensions;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.ResetPassword;

internal class ResetPasswordCommandHandler(UserManager<User> userManager)
    : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandResponse>
{
    public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        
        if (user is null)
        {
            return new ResetPasswordCommandResponse { ErrorType = BaseDomainErrorType.BadRequest };
        }
        
        // The token can be URL encoded, so we need to decode it
        var decodedToken = Uri.UnescapeDataString(request.Token!);
        
        var result = await userManager.ResetPasswordAsync(user, decodedToken, request.Password!);
        
        return result.Succeeded
            ? new ResetPasswordCommandResponse()
            : result.MapErrors<ResetPasswordCommandResponse>(BaseDomainErrorType.BadRequest);
    }
}