using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Core.Constants;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

internal class RegisterUserCommandHandler(UserManager<User> userManager, IIdentitySettings identitySettings, IEmailService emailService)
    : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = request.MapToUser();
        SetEmailAsConfirmed(user);
        
        // Save the user to the database
        IdentityResult result = await userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded)
        {
            return new RegisterUserCommandResponse()
                .AddErrors(result.Errors.Select(
                        e => new BaseDomainError { Code = e.Code, Message = e.Description }).ToList(), BaseDomainErrorType.BadRequest);
        }

        if (identitySettings is { EmailConfirmationRequired: true, SetEmailAsConfirmedDuringRegistration: false })
        {
            await SendConfirmationEmail(request, user);
        }
        
        if (request.TwoFactorEnabled == true)
        {
            await userManager.SetTwoFactorEnabledAsync(user, true);
        }

        await AddRoles(user);
        
        return new RegisterUserCommandResponse { IsSuccessRegistration = true };
    }

    private async Task SendConfirmationEmail(RegisterUserCommand request, User user)
    {
        // Generate an email confirmation token
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                
        var param = new Dictionary<string, string?>
        {
            { "token", token }, 
            { "email", user.Email }
        };
        
        var callback = QueryHelpers.AddQueryString(request.ClientUri!, param);

        EmailTemplateData emailTemplateData = new()
        {
            EmailUser = new EmailUser(Name: user.Email!, Email: user.Email!, FirstName: user.FirstName, LastName: user.LastName),
            ToAddress = user.Email!,
            Subject = "Confirm your email",
            ActionLink = callback
        };
        
        var templateFile =
            $"{Directory.GetCurrentDirectory()}/../IdentityUserManagement.Infrastructure/EmailTemplates/RegisterConfirmation.cshtml";
        
        // Send the email
        await emailService.SendAsync(emailTemplateData, templateFile);
    }

    private async Task AddRoles(User user)
        => await userManager.AddToRoleAsync(user,
            identitySettings.RegisterUserWithAdminRole ? UserRoles.Admin : UserRoles.User);
    
    private void SetEmailAsConfirmed(User user) 
        => user.EmailConfirmed = identitySettings.SetEmailAsConfirmedDuringRegistration;
}