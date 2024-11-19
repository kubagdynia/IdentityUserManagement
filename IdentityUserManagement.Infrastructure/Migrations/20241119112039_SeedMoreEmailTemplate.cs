using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityUserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreEmailTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "33d894bc-32bd-4010-a05d-50df7c354acb");

            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "3fecb42a-33c8-4590-9015-29538af12a62");

            migrationBuilder.InsertData(
                table: "EmailTemplate",
                columns: new[] { "Id", "Name", "Subject", "Template" },
                values: new object[,]
                {
                    { "3a074443-144e-42b0-aa68-eddbb6b19b31", "UserRegistration", "Welcome to our platform", "<h1>Welcome to our platform</h1>" },
                    { "4ba49cdd-9eb0-43e6-85ac-366ec8b1e38f", "TwoFactorAuthentication", "2-Factor Authentication", "@model IdentityUserManagement.Application.Dto.EmailTemplateData\r\n<br />\r\nYour 2-Factor token is: <b>@Model.ActionCode</b>" },
                    { "8d36a749-967c-42ae-b506-3952ebf5b709", "ResetPassword", "Reset your password", "@model IdentityUserManagement.Application.Dto.EmailTemplateData\r\n<br />\r\nClick <a href=\"@Model.ActionLink\">here</a> to reset your password.\r\n<br />\r\n<br />\r\nIf you did not request a password reset, please ignore this email." },
                    { "dddc7abd-9960-4774-9ba9-3960c15f24d8", "UserConfirmation", "Confirm your email", "@model  IdentityUserManagement.Application.Dto.EmailTemplateData\r\n<br />\r\nDear <b>@Model.EmailUser.FirstName @Model.EmailUser.LastName</b>,\r\n<br />\r\nClick <a href=\"@Model.ActionLink\">here</a> to confirm your email.\r\n<br />\r\n<br />\r\nIf you did not create an account, please ignore this email." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "3a074443-144e-42b0-aa68-eddbb6b19b31");

            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "4ba49cdd-9eb0-43e6-85ac-366ec8b1e38f");

            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "8d36a749-967c-42ae-b506-3952ebf5b709");

            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "dddc7abd-9960-4774-9ba9-3960c15f24d8");

            migrationBuilder.InsertData(
                table: "EmailTemplate",
                columns: new[] { "Id", "Name", "Subject", "Template" },
                values: new object[,]
                {
                    { "33d894bc-32bd-4010-a05d-50df7c354acb", "UserConfirmation", "Confirm your email", "@model  IdentityUserManagement.Application.Dto.EmailTemplateData\r\n<br />\r\nDear <b>@Model.EmailUser.FirstName @Model.EmailUser.LastName</b>,\r\n<br />\r\nClick <a href=\"@Model.ActionLink\">here</a> to confirm your email.\r\n<br />\r\n<br />\r\nIf you did not create an account, please ignore this email." },
                    { "3fecb42a-33c8-4590-9015-29538af12a62", "UserRegistration", "Welcome to our platform", "<h1>Welcome to our platform</h1>" }
                });
        }
    }
}
