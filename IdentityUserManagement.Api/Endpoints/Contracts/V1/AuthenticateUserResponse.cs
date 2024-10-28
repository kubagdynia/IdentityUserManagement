namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record AuthenticateUserResponse(bool IsAuthSuccessful, string? Token = null, bool? RequiresTwoFactor = null, string? Provider = null);