namespace IdentityUserManagement.Api.Endpoints.Contracts.V1;

public record ConfirmEmailRequest(string Email, string Token);