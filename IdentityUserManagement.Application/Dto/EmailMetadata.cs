namespace IdentityUserManagement.Application.Dto;

public record EmailMetadata(string ToAddress, string Subject, string? Body = "", string? AttachmentPath = "");