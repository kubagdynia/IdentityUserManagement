using IdentityUserManagement.Core.Constants;
using IdentityUserManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler(UserManager<User> userManager) : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = request.MapToUser();
        
        // Save the user to the database
        IdentityResult result = await userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded)
        {
            var response = new RegisterUserCommandResponse { IsSuccessRegistration = false};
            response.AddErrors(result.Errors.Select(e => e.Description));
            return response;
        }

        await userManager.AddToRoleAsync(user, UserRoles.User);
        
        return new RegisterUserCommandResponse { IsSuccessRegistration = true };
    }
}