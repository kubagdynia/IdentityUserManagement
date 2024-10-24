namespace IdentityUserManagement.Application.Common;

public record BaseDomainError
{
    public string Code { get; set; }
    
    public string Message { get; set; }
}