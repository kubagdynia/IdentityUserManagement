using IdentityUserManagement.Application.Dto;

namespace IdentityUserManagement.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailMetadata emailMetadata);
    
    Task SendAsync(List<EmailMetadata> emailsMetadata);
    
    Task SendAsync(EmailMetadata emailMetadata, EmailUser emailUser, string templateFile);
    
    Task SendAsync(List<EmailMetadata> emailsMetadata, EmailUser emailUser, string templateFile);
}