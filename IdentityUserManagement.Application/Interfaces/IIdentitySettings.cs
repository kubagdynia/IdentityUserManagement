namespace IdentityUserManagement.Application.Interfaces;

public interface IIdentitySettings
{
    bool RegisterUserWithAdminRole { get; }
    
    bool EmailConfirmationRequired { get; }
    
    bool SetEmailAsConfirmedDuringRegistration { get; }
    
    int TokenLifespanInHours { get; }
}