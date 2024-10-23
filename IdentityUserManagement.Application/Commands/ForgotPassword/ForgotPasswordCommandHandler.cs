using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityUserManagement.Application.Commands.ForgotPassword;

internal class ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService)
    : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandResponse>
{
    public async Task<ForgotPasswordCommandResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        
        if (user is null)
        {
            return new ForgotPasswordCommandResponse
                { ErrorType = BaseDomainErrorType.BadRequest };
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?>
        {
            { "token", token }, 
            { "email", request.Email! }
        };
        
        var callback = QueryHelpers.AddQueryString(request.ClientUri!, param);
        
        EmailMetadata emailMetadata = new(
                
            ToAddress: request.Email!,
            Subject: "Reset your password",
            Body: $@"Click <a href=""{callback}"">here</a> to reset your password.

If you did not request a password reset, please ignore this email." );
        
        // Send the email
        await emailService.SendAsync(emailMetadata);
        
        return new ForgotPasswordCommandResponse();
    }
}