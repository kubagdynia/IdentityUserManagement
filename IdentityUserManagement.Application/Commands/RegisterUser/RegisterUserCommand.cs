using MediatR;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserCommandResponse>
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public string? ConfirmPassword { get; set; }
    
    public string? ClientUri { get; set; }
}