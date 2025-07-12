using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => new { x.TenantId, x.UserId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => new { x.TenantId, x.UserId, x.SalesId });
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => new { x.TenantId, x.UserId, x.ExpensesId });
                    table.ForeignKey(
                        name: "FK_Expenses_Sales_SalesTenantId_SalesUserId_SalesId",
                        columns: x => new { x.SalesTenantId, x.SalesUserId, x.SalesId },
                        principalTable: "Sales",
                        principalColumns: new[] { "TenantId", "UserId", "SalesId" });
                });

            migrationBuilder.CreateTable(
                name: "ServiceFacilitiesAgreements",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceFacilityAgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpensesEntityTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpensesEntityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpensesEntityExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesEntityTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesEntityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesEntitySalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFacilitiesAgreements", x => new { x.TenantId, x.UserId, x.ServiceFacilityAgreementId });
                    table.ForeignKey(
                        name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                        columns: x => new { x.ExpensesEntityTenantId, x.ExpensesEntityUserId, x.ExpensesEntityExpensesId },
                        principalTable: "Expenses",
                        principalColumns: new[] { "TenantId", "UserId", "ExpensesId" });
                    table.ForeignKey(
                        name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                        columns: x => new { x.SalesEntityTenantId, x.SalesEntityUserId, x.SalesEntitySalesId },
                        principalTable: "Sales",
                        principalColumns: new[] { "TenantId", "UserId", "SalesId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SalesTenantId_SalesUserId_SalesId",
                table: "Expenses",
                columns: new[] { "SalesTenantId", "SalesUserId", "SalesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_ExpensesEntityTenantId_ExpensesEntityUserId_ExpensesEntityExpensesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "ExpensesEntityTenantId", "ExpensesEntityUserId", "ExpensesEntityExpensesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFacilitiesAgreements_SalesEntityTenantId_SalesEntityUserId_SalesEntitySalesId",
                table: "ServiceFacilitiesAgreements",
                columns: new[] { "SalesEntityTenantId", "SalesEntityUserId", "SalesEntitySalesId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "ServiceFacilitiesAgreements");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
