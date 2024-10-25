using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Infrastructure.Identity;

public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
    {
        var username = await manager.GetUserNameAsync(user);
        
        if (string.Equals(username, password, StringComparison.OrdinalIgnoreCase))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordAndUsernameMatch",
                Description = "Password cannot be the same as username"
            });
        }
        
        if (password is not null && password.Contains("password", StringComparison.OrdinalIgnoreCase))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordContainsPassword",
                Description = "Password cannot contain the word 'password'"
            });
        }
        
        return IdentityResult.Success;
    }
}