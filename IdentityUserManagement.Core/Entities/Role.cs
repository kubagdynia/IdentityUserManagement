using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Core.Entities;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}