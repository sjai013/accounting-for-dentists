using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class AddDateContainerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "ContractIncome");

            migrationBuilder.AddColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DateReferences",
                columns: table => new
                {
                    DateContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateReferences", x => x.DateContainerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_DateReferenceDateContainerId",
                table: "Sales",
                column: "DateReferenceDateContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_DateReferenceDateContainerId",
                table: "Expenses",
                column: "DateReferenceDateContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                column: "InvoiceDateReferenceDateContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_DateReferences_TenantId_UserId",
                table: "DateReferences",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractIncome_DateReferences_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                column: "InvoiceDateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_DateReferences_DateReferenceDateContainerId",
                table: "Expenses",
                column: "DateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_DateReferences_DateReferenceDateContainerId",
                table: "Sales",
                column: "DateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractIncome_DateReferences_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_DateReferences_DateReferenceDateContainerId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_DateReferences_DateReferenceDateContainerId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "DateReferences");

            migrationBuilder.DropIndex(
                name: "IX_Sales_DateReferenceDateContainerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_DateReferenceDateContainerId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_ContractIncome_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome");

            migrationBuilder.DropColumn(
                name: "DateReferenceDateContainerId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DateReferenceDateContainerId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "InvoiceDateReferenceDateContainerId",
                table: "ContractIncome");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Sales",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Expenses",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "InvoiceDate",
                table: "ContractIncome",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
