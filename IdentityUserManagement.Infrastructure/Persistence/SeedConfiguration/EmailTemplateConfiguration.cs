using IdentityUserManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityUserManagement.Infrastructure.Persistence.SeedConfiguration;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasData(
            new EmailTemplate
            {
                Name = "UserRegistration",
                Subject = "Welcome to our platform",
                Template = "<h1>Welcome to our platform</h1>"
            },
            new EmailTemplate
            {
                Name = "UserConfirmation",
                Subject = "Confirm your email",
                Template = @"@model  IdentityUserManagement.Application.Dto.EmailTemplateData
<br />
Dear <b>@Model.EmailUser.FirstName @Model.EmailUser.LastName</b>,
<br />
Click <a href=""@Model.ActionLink"">here</a> to confirm your email.
<br />
<br />
If you did not create an account, please ignore this email."
            }
        );
    }
}