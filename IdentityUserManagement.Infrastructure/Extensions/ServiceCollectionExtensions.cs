using IdentityUserManagement.Application.Security;
using IdentityUserManagement.Core.Entities;
using IdentityUserManagement.Infrastructure.Persistence;
using IdentityUserManagement.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityUserManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
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
            .AddEntityFrameworkStores<IdentityUserManagementDbContext>();

        services.AddSingleton<IJwtHandler, JwtHandler>();

        return services;
    }
}