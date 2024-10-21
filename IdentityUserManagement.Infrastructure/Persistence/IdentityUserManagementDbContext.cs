using IdentityUserManagement.Core.Entities;
using IdentityUserManagement.Infrastructure.Persistence.SeedConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityUserManagement.Infrastructure.Persistence;

public class IdentityUserManagementDbContext(DbContextOptions options) : IdentityDbContext<User, Role, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Seed data
        builder.ApplyConfiguration(new RoleConfiguration());
    }
}