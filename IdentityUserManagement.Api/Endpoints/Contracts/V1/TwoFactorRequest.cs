namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record TwoFactorRequest(string? Email, string? Provider, string? Token);