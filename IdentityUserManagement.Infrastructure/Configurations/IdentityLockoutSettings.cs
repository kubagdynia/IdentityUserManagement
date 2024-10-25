namespace IdentityUserManagement.Infrastructure.Configurations;

public record IdentityLockoutSettings
{
    public bool AllowedForNewUsers { get; init; } = true;
    
    public int DefaultLockoutTimeInMinutes { get; init; } = 5;
    
    public int MaxFailedAccessAttempts { get; init; } = 3;
};