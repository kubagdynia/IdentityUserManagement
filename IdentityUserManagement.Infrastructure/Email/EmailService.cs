using FluentEmail.Core;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Interfaces;

namespace IdentityUserManagement.Infrastructure.Email;

internal class EmailService(IFluentEmail fluentEmail, IFluentEmailFactory fluentEmailFactory) : IEmailService
{
    public async Task SendAsync(EmailMetadata emailMetadata)
    {
        var email = fluentEmail
            .To(emailMetadata.ToAddress)
            .Subject(emailMetadata.Subject)
            .Body(emailMetadata.Body);
        
        AddAttachment(emailMetadata, email);
        
        await email.SendAsync();
    }

    public async Task SendAsync(List<EmailMetadata> emailsMetadata)
    {
        foreach (var emailMetadata in emailsMetadata)
        {
            var email = fluentEmailFactory.Create()
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body);
        
            AddAttachment(emailMetadata, email);
        
            await email.SendAsync();
        }
    }

    public async Task SendAsync(EmailMetadata emailMetadata, EmailUser emailUser, string templateFile)
    {
        var email = fluentEmail
            .To(emailMetadata.ToAddress)
            .Subject(emailMetadata.Subject)
            .UsingTemplateFromFile(templateFile, emailUser);
            
        AddAttachment(emailMetadata, email);
        
        await email.SendAsync();
    }

    public async Task SendAsync(List<EmailMetadata> emailsMetadata, EmailUser emailUser, string templateFile)
    {
        foreach (var emailMetadata in emailsMetadata)
        {
            var email = fluentEmailFactory.Create()
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .UsingTemplateFromFile(templateFile, emailUser);
            
            AddAttachment(emailMetadata, email);
        
            await email.SendAsync();
        }
    }
    
    private static void AddAttachment(EmailMetadata emailMetadata, IFluentEmail email)
    {
        if (!string.IsNullOrEmpty(emailMetadata.AttachmentPath))
        {
            email.AttachFromFilename(emailMetadata.AttachmentPath,
                attachmentName: Path.GetFileName(emailMetadata.AttachmentPath));
        }
    }
}