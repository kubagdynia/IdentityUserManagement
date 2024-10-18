using System.Text;
using IdentityUserManagement.Api.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace IdentityUserManagement.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JWTSettings");
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value!))
            };
        });

        builder.Services
            .AddAuthorization()
            .AddEndpointsApiExplorer() // Add the endpoint API explorer for generating OpenAPI document.
            .AddSwaggerGen(opt => opt.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

        return builder;
    }
}