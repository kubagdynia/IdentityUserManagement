using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Core.Constants;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

internal class RegisterUserCommandHandler(UserManager<User> userManager, IIdentitySettings identitySettings)
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
            var response = new RegisterUserCommandResponse { IsSuccessRegistration = false };
            response.AddErrors(result.Errors.Select(e => e.Description));
            return response;
        }

        await AddRoles(user);
        
        return new RegisterUserCommandResponse { IsSuccessRegistration = true };
    }

    private async Task AddRoles(User user)
        => await userManager.AddToRoleAsync(user,
            identitySettings.RegisterUserWithAdminRole ? UserRoles.Admin : UserRoles.User);
    
    private void SetEmailAsConfirmed(User user) 
        => user.EmailConfirmed = identitySettings.SetEmailAsConfirmedDuringRegistration;
}