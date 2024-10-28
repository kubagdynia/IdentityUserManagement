namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record TwoFactorResponse(bool IsAuthSuccessful, string? Token = null);