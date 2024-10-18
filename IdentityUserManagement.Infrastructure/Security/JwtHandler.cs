using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityUserManagement.Infrastructure.Security;

public class JwtHandler(IConfiguration configuration) : IJwtHandler
{
    private readonly IConfigurationSection _jwtSettings = configuration.GetSection("JWTSettings");
    
    public string GenerateToken(User user, IList<string> roles)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user, roles);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!));
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    private List<Claim> GetClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        
        // Add roles to the claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}