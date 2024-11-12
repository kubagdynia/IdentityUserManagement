using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityUserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectToEmailTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EmailTemplate",
                type: "TEXT",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EmailTemplate");
        }
    }
}
