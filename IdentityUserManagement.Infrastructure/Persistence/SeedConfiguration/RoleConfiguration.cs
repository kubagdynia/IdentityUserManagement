using IdentityUserManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityUserManagement.Infrastructure.Persistence.SeedConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = "d3b07384-d9a0-4c9b-8f3d-2b0a7b1a1e8e",
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role"
            },
            new Role
            {
                Id = "e7b8f8e2-3c4a-4d8b-9f1a-2d3b4c5d6e7f",
                Name = "User",
                NormalizedName = "USER",
                Description = "User role"
            }
        );
    }
}