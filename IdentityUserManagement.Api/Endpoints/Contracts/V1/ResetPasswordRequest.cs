namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record ResetPasswordRequest(string Password, string ConfirmPassword, string? Email, string? Token);