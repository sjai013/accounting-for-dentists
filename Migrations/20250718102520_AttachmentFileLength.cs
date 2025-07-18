using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentFileLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeBytes",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeBytes",
                table: "Attachments");
        }
    }
}
