using MediatR;

namespace IdentityUserManagement.Application.Commands.AuthenticateUser;

public class AuthenticateUserCommand : IRequest<AuthenticateUserCommandResponse>
{
    public string? Email { get; set; }
    
    public string? Password { get; set; }
}