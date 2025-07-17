using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingForDentists.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpensesId);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SalesId);
                });

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
                name: "IX_Businesses_TenantId_UserId",
                table: "Businesses",
                columns: new[] { "TenantId", "UserId" });

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

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TenantId_UserId",
                table: "Expenses",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UserId_TenantId",
                table: "Sales",
                columns: new[] { "UserId", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "ContractIncome");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
