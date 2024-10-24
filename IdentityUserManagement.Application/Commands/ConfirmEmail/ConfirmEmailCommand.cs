using MediatR;

namespace IdentityUserManagement.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<ConfirmEmailCommandResponse>
{
    public string? Email { get; set; }
    
    public string? Token { get; set; }
}