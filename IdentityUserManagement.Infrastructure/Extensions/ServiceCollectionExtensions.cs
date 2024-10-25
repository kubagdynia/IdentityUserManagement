using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using IdentityUserManagement.Infrastructure.Configurations;
using IdentityUserManagement.Infrastructure.Persistence;
using IdentityUserManagement.Infrastructure.Security;
using IdentityUserManagement.Infrastructure.Email;
using IdentityUserManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityUserManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterJwtSettings(services, configuration);
        IdentitySettings identitySettings = RegisterIdentitySettings(services, configuration);

        services.AddDbContext<IdentityUserManagementDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = identitySettings.Password.RequiredLength;
                opt.Password.RequireDigit = identitySettings.Password.RequireDigit;
                opt.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
                opt.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
                opt.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;

                opt.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;
                opt.Lockout.DefaultLockoutTimeSpan =
                    TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeInMinutes);
                opt.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;
            })
            .AddEntityFrameworkStores<IdentityUserManagementDbContext>()
            .AddDefaultTokenProviders()
            .AddPasswordValidator<CustomPasswordValidator<User>>();
        
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromHours(identitySettings.TokenLifespanInHours); // Default is 1 day
        });

        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
        
        services.AddFluentEmail(configuration);

        return services;
    }

    private static void RegisterJwtSettings(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSettings);
    }

    private static IdentitySettings RegisterIdentitySettings(IServiceCollection services, IConfiguration configuration)
    {
        var identitySettings = configuration.GetSection(IdentitySettings.SectionName).Get<IdentitySettings>()!;
        
        // Register IdentitySettings as an implementation of IIdentitySettings
        services.AddSingleton<IIdentitySettings>(_ => identitySettings);

        return identitySettings;
    }
}