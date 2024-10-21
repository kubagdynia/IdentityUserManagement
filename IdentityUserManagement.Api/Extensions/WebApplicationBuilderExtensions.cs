using System.Text;
using IdentityUserManagement.Api.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace IdentityUserManagement.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        ConfigureAuthentication(builder);
        ConfigureAuthorization(builder);
        ConfigureSwaggerAndExceptionHandling(builder);
        
        return builder;
    }
    
    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JWTSettings");
        var securityKey = jwtSettings["securityKey"] ?? throw new ArgumentNullException("securityKey");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                ClockSkew = TimeSpan.Zero // Remove delay for token expiration
            };
        });
    }
    
    private static void ConfigureAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

            options.AddPolicy("Admin", policy =>
                policy.RequireRole("Admin").AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
        });
    }
    
    private static void ConfigureSwaggerAndExceptionHandling(WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer() // Add the endpoint API explorer for generating OpenAPI document.
            .AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();
    }
}