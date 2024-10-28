using MediatR;

namespace IdentityUserManagement.Application.Commands.TwoFactor;

public class TwoFactorCommand : IRequest<TwoFactorCommandResponse>
{
    public string? Email { get; set; }
    
    public string? Provider { get; set; }
    
    public string? Token { get; set; }
}