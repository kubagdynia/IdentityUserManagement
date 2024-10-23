using IdentityUserManagement.Application.Interfaces;
using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using IdentityUserManagement.Infrastructure.Configurations;
using IdentityUserManagement.Infrastructure.Persistence;
using IdentityUserManagement.Infrastructure.Security;
using IdentityUserManagement.Infrastructure.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityUserManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterJwtSettings(services, configuration);
        RegisterIdentitySettings(services, configuration);

        services.AddDbContext<IdentityUserManagementDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<IdentityUserManagementDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromHours(2); // Default is 1 day
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

    private static void RegisterIdentitySettings(IServiceCollection services, IConfiguration configuration)
    {
        // Bind IdentitySettings from appsettings.json
        services.Configure<IdentitySettings>(configuration.GetSection(IdentitySettings.SectionName));
        
        // Register IdentitySettings as an implementation of IIdentitySettings
        services.AddSingleton<IIdentitySettings>(sp => 
            sp.GetRequiredService<IOptions<IdentitySettings>>().Value);
    }
}