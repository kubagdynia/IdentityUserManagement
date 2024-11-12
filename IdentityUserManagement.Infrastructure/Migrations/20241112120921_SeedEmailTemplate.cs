using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityUserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmailTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmailTemplate",
                columns: new[] { "Id", "Name", "Subject", "Template" },
                values: new object[,]
                {
                    { "33d894bc-32bd-4010-a05d-50df7c354acb", "UserConfirmation", "Confirm your email", "@model  IdentityUserManagement.Application.Dto.EmailTemplateData\r\n<br />\r\nDear <b>@Model.EmailUser.FirstName @Model.EmailUser.LastName</b>,\r\n<br />\r\nClick <a href=\"@Model.ActionLink\">here</a> to confirm your email.\r\n<br />\r\n<br />\r\nIf you did not create an account, please ignore this email." },
                    { "3fecb42a-33c8-4590-9015-29538af12a62", "UserRegistration", "Welcome to our platform", "<h1>Welcome to our platform</h1>" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "33d894bc-32bd-4010-a05d-50df7c354acb");

            migrationBuilder.DeleteData(
                table: "EmailTemplate",
                keyColumn: "Id",
                keyValue: "3fecb42a-33c8-4590-9015-29538af12a62");
        }
    }
}
