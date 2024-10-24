using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Extensions;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.ConfirmEmail;

internal class ConfirmEmailCommandHandler(UserManager<User> userManager)
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCommandResponse>
{
    public async Task<ConfirmEmailCommandResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        
        if (user is null)
        {
            return new ConfirmEmailCommandResponse { ErrorType = BaseDomainErrorType.BadRequest };
        }
        
        // The token can be URL encoded, so we need to decode it
        var decodedToken = Uri.UnescapeDataString(request.Token!);
        
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);

        return result.Succeeded
            ? new ConfirmEmailCommandResponse()
            : result.MapErrors<ConfirmEmailCommandResponse>(BaseDomainErrorType.BadRequest);
    }
}