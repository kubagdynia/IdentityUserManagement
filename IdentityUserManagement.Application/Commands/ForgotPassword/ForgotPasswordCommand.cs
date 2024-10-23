using MediatR;

namespace IdentityUserManagement.Application.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest<ForgotPasswordCommandResponse>
{
    public string? Email { get; set; }
    
    public string? ClientUri { get; set; }
}