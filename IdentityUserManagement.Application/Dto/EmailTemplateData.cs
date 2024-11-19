namespace IdentityUserManagement.Application.Dto;

public record EmailTemplateData
{
    public string? ToAddress { get; set; }
    public string? Subject { get; set; }
    public EmailUser? EmailUser { get; set; }
    public string? AttachmentPath { get; set; }
    public string? ActionLink { get; set; }
    public string? ActionCode { get; set; }
};