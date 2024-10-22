namespace IdentityUserManagement.Infrastructure.Configurations;

public record JwtSettings
{
    public const string SectionName = "JWTSettings";
    
    public string SecurityKey { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public int ExpiryInMinutes { get; set; } = 5;
}