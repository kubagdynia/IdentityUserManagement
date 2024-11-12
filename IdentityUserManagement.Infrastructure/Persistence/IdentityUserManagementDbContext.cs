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

        // Configure entities
        builder.Entity<EmailTemplate>()
            .HasIndex(e => e.Name).IsUnique();

        builder.Entity<EmailTemplate>()
            .Property(e => e.Name).HasMaxLength(256);
        builder.Entity<EmailTemplate>()
            .Property(e => e.Subject).HasMaxLength(256);
        
        // Seed data
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new EmailTemplateConfiguration());
    }
    
    public DbSet<EmailTemplate> EmailTemplate { get; set; }
}