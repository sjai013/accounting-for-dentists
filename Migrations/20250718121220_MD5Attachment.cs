using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class MD5Attachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MD5Hash",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MD5Hash",
                table: "Attachments");
        }
    }
}
