namespace IdentityUserManagement.Core.Entities;

public class EmailTemplate
{
    public EmailTemplate()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Subject { get; set; }
    
    public string Template { get; set; }
}