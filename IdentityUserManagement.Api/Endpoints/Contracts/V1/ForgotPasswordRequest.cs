namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record ForgotPasswordRequest(string? Email, string? ClientUri);