namespace IdentityUserManagement.Application.Dto;

public record RegistrationResponseDto(bool IsSuccessRegistration, IEnumerable<string>? Errors = null);