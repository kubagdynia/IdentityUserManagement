using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Core.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
}