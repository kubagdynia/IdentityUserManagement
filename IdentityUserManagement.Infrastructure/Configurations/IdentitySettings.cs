using IdentityUserManagement.Application.Interfaces;

namespace IdentityUserManagement.Infrastructure.Configurations;

public record IdentitySettings : IIdentitySettings
{
    public const string SectionName = "IdentitySettings";
    
    public bool RegisterUserWithAdminRole { get; init; }
    
    public bool EmailConfirmationRequired { get; init; } = true;

    public bool SetEmailAsConfirmedDuringRegistration { get; init; }
    
    public int TokenLifespanInHours { get; init; } = 24;

    public IdentityPasswordSettings Password { get; init; } = new();
    
    public IdentityLockoutSettings Lockout { get; init; } = new();
};