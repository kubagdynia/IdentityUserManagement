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
        
        AddAttachment(emailMetadata.AttachmentPath, email);
        
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
        
            AddAttachment(emailMetadata.AttachmentPath, email);
        
            await email.SendAsync();
        }
    }

    public async Task SendAsync(EmailMetadata emailMetadata, EmailUser emailUser, string templateFile)
    {
        var email = fluentEmail
            .To(emailMetadata.ToAddress)
            .Subject(emailMetadata.Subject)
            .UsingTemplateFromFile(templateFile, emailUser);
            
        AddAttachment(emailMetadata.AttachmentPath, email);
        
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
            
            AddAttachment(emailMetadata.AttachmentPath, email);
        
            await email.SendAsync();
        }
    }

    public async Task SendAsync(EmailTemplateData emailTemplateData, string templateFile)
    {
        var email = fluentEmail
            .To(emailTemplateData.ToAddress)
            .Subject(emailTemplateData.Subject)
            .UsingTemplateFromFile(templateFile, emailTemplateData);
            
        AddAttachment(emailTemplateData.AttachmentPath, email);
        
        await email.SendAsync();
    }

    private static void AddAttachment(string? attachmentPath, IFluentEmail email)
    {
        if (!string.IsNullOrEmpty(attachmentPath))
        {
            email.AttachFromFilename(attachmentPath,
                attachmentName: Path.GetFileName(attachmentPath));
        }
    }
}