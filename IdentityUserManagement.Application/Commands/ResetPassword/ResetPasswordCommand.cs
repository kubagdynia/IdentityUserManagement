using MediatR;

namespace IdentityUserManagement.Application.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<ResetPasswordCommandResponse>
{
    public string? Password { get; set; }
    
    public string? ConfirmPassword { get; set; }
    
    public string? Email { get; set; }
    
    public string? Token { get; set; }
}