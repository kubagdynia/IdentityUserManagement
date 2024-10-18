using IdentityUserManagement.Core.Entities;

namespace IdentityUserManagement.Application.Security;

public interface IJwtHandler
{
    string GenerateToken(User user, IList<string> roles);
}