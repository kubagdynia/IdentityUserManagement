using IdentityUserManagement.Core.Entities;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

public static class RegisterUserMappers
{
    public static User MapToUser(this RegisterUserCommand registerUserCommand)
    {
        return new User
        {
            UserName = registerUserCommand.Email,
            Email = registerUserCommand.Email,
            FirstName = registerUserCommand.FirstName,
            LastName = registerUserCommand.LastName
        };
    }
}