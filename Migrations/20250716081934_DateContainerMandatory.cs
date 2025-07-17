using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class DateContainerMandatory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractIncome_DateReferences_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                column: "InvoiceDateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_DateReferences_DateReferenceDateContainerId",
                table: "Expenses",
                column: "DateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_DateReferences_DateReferenceDateContainerId",
                table: "Sales",
                column: "DateReferenceDateContainerId",
                principalTable: "DateReferences",
                principalColumn: "DateContainerId",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DateReferenceDateContainerId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
    }
}
