using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "ContractIncome",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_AttachmentId",
                table: "Sales",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AttachmentId",
                table: "Expenses",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_AttachmentId",
                table: "ContractIncome",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantId_UserId",
                table: "Attachments",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractIncome_Attachments_AttachmentId",
                table: "ContractIncome",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Attachments_AttachmentId",
                table: "Expenses",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Attachments_AttachmentId",
                table: "Sales",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractIncome_Attachments_AttachmentId",
                table: "ContractIncome");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Attachments_AttachmentId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Attachments_AttachmentId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Sales_AttachmentId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AttachmentId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_ContractIncome_AttachmentId",
                table: "ContractIncome");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "ContractIncome");
        }
    }
}
