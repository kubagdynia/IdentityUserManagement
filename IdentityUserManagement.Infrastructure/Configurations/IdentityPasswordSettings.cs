namespace IdentityUserManagement.Infrastructure.Configurations;

public record IdentityPasswordSettings
{
    public int RequiredLength { get; init; } = 8;

    public bool RequireDigit { get; init; } = true;

    public bool RequireUppercase { get; init; } = true;

    public bool RequireLowercase { get; init; } = true;
    
    public bool RequireNonAlphanumeric { get; init; } = true;
}