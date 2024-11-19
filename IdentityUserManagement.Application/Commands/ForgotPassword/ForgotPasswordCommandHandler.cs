using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Core.Constants;
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
        
        EmailTemplateData emailTemplateData = new()
        {
            EmailUser = new EmailUser(Name: user.Email!, Email: user.Email!, FirstName: user.FirstName, LastName: user.LastName),
            ToAddress = user.Email!,
            ActionLink = callback
        };
        
        // Send the email
        await emailService.SendAsync(emailTemplateData, EmailTemplateType.ResetPassword);
        
        return new ForgotPasswordCommandResponse();
    }
}