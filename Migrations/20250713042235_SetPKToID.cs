using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class SetPKToID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Sales_SalesTenantId_SalesUserId_SalesId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceFacilitiesAgreements",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropIndex(
                name: "IX_ServiceFacilitiesAgreements_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropIndex(
                name: "IX_ServiceFacilitiesAgreements_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SalesTenantId_SalesUserId_SalesId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesEntityTenantId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropColumn(
                name: "ExpensesEntityUserId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropColumn(
                name: "SalesEntityTenantId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropColumn(
                name: "SalesEntityUserId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropColumn(
                name: "SalesTenantId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SalesUserId",
                table: "Expenses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceFacilitiesAgreements",
                table: "ServiceFacilitiesAgreements",
                column: "ServiceFacilityAgreementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "SalesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "ExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements",
                column: "ExpensesEntityExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements",
                column: "SalesEntitySalesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_TenantId_UserId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UserId_TenantId",
                table: "Sales",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SalesId",
                table: "Expenses",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TenantId_UserId",
                table: "Expenses",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_TenantId_UserId",
                table: "Businesses",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Sales_SalesId",
                table: "Expenses",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements",
                column: "ExpensesEntityExpensesId",
                principalTable: "Expenses",
                principalColumn: "ExpensesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements",
                column: "SalesEntitySalesId",
                principalTable: "Sales",
                principalColumn: "SalesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Sales_SalesId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceFacilitiesAgreements",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropIndex(
                name: "IX_ServiceFacilitiesAgreements_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropIndex(
                name: "IX_ServiceFacilitiesAgreements_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropIndex(
                name: "IX_ServiceFacilitiesAgreements_TenantId_UserId",
                table: "ServiceFacilitiesAgreements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_UserId_TenantId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SalesId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TenantId_UserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_TenantId_UserId",
                table: "Businesses");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpensesEntityTenantId",
                table: "ServiceFacilitiesAgreements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpensesEntityUserId",
                table: "ServiceFacilitiesAgreements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesEntityTenantId",
                table: "ServiceFacilitiesAgreements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesEntityUserId",
                table: "ServiceFacilitiesAgreements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesTenantId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesUserId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceFacilitiesAgreements",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "TenantId", "UserId", "ServiceFacilityAgreementId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                columns: new[] { "TenantId", "UserId", "SalesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                columns: new[] { "TenantId", "UserId", "ExpensesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "ExpensesEntityTenantId", "ExpensesEntityUserId", "ExpensesEntityExpensesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "SalesEntityTenantId", "SalesEntityUserId", "SalesEntitySalesId" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SalesTenantId_SalesUserId_SalesId",
                table: "Expenses",
                columns: new[] { "SalesTenantId", "SalesUserId", "SalesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Sales_SalesTenantId_SalesUserId_SalesId",
                table: "Expenses",
                columns: new[] { "SalesTenantId", "SalesUserId", "SalesId" },
                principalTable: "Sales",
                principalColumns: new[] { "TenantId", "UserId", "SalesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "ExpensesEntityTenantId", "ExpensesEntityUserId", "ExpensesEntityExpensesId" },
                principalTable: "Expenses",
                principalColumns: new[] { "TenantId", "UserId", "ExpensesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "SalesEntityTenantId", "SalesEntityUserId", "SalesEntitySalesId" },
                principalTable: "Sales",
                principalColumns: new[] { "TenantId", "UserId", "SalesId" });
        }
    }
}
