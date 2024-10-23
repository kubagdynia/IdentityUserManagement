using IdentityUserManagement.Core.Entities;

namespace IdentityUserManagement.Application.Security;

public interface ITokenGenerator
{
    string GenerateToken(User user, IList<string> roles);
}