using IdentityUserManagement.Application.Common;

namespace IdentityUserManagement.Application.Commands.RegisterUser;

public class RegisterUserCommandResponse : BaseDomainResponse
{
    public bool IsSuccessRegistration { get; set; }
}