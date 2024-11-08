namespace IdentityUserManagement.Application.Dto;

public record EmailUser(string Name, string Email, string? FirstName = null, string? LastName = null);