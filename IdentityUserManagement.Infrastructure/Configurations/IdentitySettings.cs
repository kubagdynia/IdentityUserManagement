using IdentityUserManagement.Application.Interfaces;

namespace IdentityUserManagement.Infrastructure.Configurations;

public record IdentitySettings : IIdentitySettings
{
    public const string SectionName = "IdentitySettings";
    
    public bool RegisterUserWithAdminRole { get; set; }
    
    public bool EmailConfirmationRequired { get; set; }

    public bool SetEmailAsConfirmedDuringRegistration { get; set; }
};