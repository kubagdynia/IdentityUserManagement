using IdentityUserManagement.Application.Common;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

public class AuthenticateUserCommandResponse : BaseDomainResponse
{
    public bool IsAuthSuccessful { get; set; }
    
    public string? Token { get; set; }
}