using System.Text;
using IdentityUserManagement.Api.ExceptionHandling;
using IdentityUserManagement.Core.Constants;
using IdentityUserManagement.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Serilog;

namespace IdentityUserManagement.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        ConfigureSerilog(builder);
        ConfigureAuthentication(builder);
        ConfigureAuthorization(builder);
        ConfigureSwaggerAndExceptionHandling(builder);
        
        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        
        return builder;
    }

    private static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        
        // configure logging
        builder.Services.AddLogging(b =>
        { 
            b.AddSerilog();
        });
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwtSettings = GetJwtSettings(builder);

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
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                ClockSkew = TimeSpan.Zero // Remove delay for token expiration
            };
        });
    }

    private static JwtSettings GetJwtSettings(WebApplicationBuilder builder)
    {
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
        return jwtSettings;
    }

    private static void ConfigureAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

            options.AddPolicy(UserRoles.Admin, policy =>
                policy.RequireRole(UserRoles.Admin).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
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