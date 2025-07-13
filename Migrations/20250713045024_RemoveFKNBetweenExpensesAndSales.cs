using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKNBetweenExpensesAndSales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Sales_SalesId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SalesId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "Expenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SalesId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SalesId",
                table: "Expenses",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Sales_SalesId",
                table: "Expenses",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "SalesId");
        }
    }
}
