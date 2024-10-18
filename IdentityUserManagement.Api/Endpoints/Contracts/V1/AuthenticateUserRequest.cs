namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record AuthenticateUserRequest(string? Email, string? Password);