using IdentityUserManagement.Core.Entities;
using IdentityUserManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityUserManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityUserManagementDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityUserManagementDbContext>();

        // services.AddIdentity<IdentityUser, IdentityRole>()
        //     .AddEntityFrameworkStores<IdentityDbContext>()
        //     .AddDefaultTokenProviders();
        //
        // services.AddScoped<ICurrentUserService, CurrentUserService>();
        // services.AddTransient<IDateTime, DateTimeService>();
    }
}