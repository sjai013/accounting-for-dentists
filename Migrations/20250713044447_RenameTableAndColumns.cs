using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceFacilitiesAgreements");

            migrationBuilder.CreateTable(
                name: "ContractIncome",
                columns: table => new
                {
                    ContractualAgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpensesEntityExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesEntitySalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractIncome", x => x.ContractualAgreementId);
                    table.ForeignKey(
                        name: "FK_ContractIncome_Expenses_ExpensesEntityExpensesId",
                        column: x => x.ExpensesEntityExpensesId,
                        principalTable: "Expenses",
                        principalColumn: "ExpensesId");
                    table.ForeignKey(
                        name: "FK_ContractIncome_Sales_SalesEntitySalesId",
                        column: x => x.SalesEntitySalesId,
                        principalTable: "Sales",
                        principalColumn: "SalesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_ExpensesEntityExpensesId",
                table: "ContractIncome",
                column: "ExpensesEntityExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_SalesEntitySalesId",
                table: "ContractIncome",
                column: "SalesEntitySalesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_TenantId_UserId",
                table: "ContractIncome",
                columns: new[] { "TenantId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractIncome");

            migrationBuilder.CreateTable(
                name: "ServiceFacilitiesAgreements",
                columns: table => new
                {
                    ServiceFacilityAgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensesEntityExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesEntitySalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFacilitiesAgreements", x => x.ServiceFacilityAgreementId);
                    table.ForeignKey(
                        name: "FK_ServiceFacilitiesAgreements_Expenses_ExpensesEntityExpensesId",
                        column: x => x.ExpensesEntityExpensesId,
                        principalTable: "Expenses",
                        principalColumn: "ExpensesId");
                    table.ForeignKey(
                        name: "FK_ServiceFacilitiesAgreements_Sales_SalesEntitySalesId",
                        column: x => x.SalesEntitySalesId,
                        principalTable: "Sales",
                        principalColumn: "SalesId");
                });

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
        }
    }
}
