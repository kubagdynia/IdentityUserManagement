using IdentityUserManagement.Application.Common;

namespace IdentityUserManagement.Application.Commands.TwoFactor;

public class TwoFactorCommandResponse : BaseDomainResponse
{
    public bool IsAuthSuccessful { get; set; }
    
    public string? Token { get; set; }
}