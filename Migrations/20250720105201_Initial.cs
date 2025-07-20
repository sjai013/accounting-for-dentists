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
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerFilename = table.Column<string>(type: "TEXT", nullable: false),
                    SizeBytes = table.Column<int>(type: "INTEGER", nullable: false),
                    MD5Hash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "DateReferences",
                columns: table => new
                {
                    DateContainerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateReferences", x => x.DateContainerId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpensesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateReferenceDateContainerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BusinessName = table.Column<string>(type: "TEXT", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpensesId);
                    table.ForeignKey(
                        name: "FK_Expenses_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentId");
                    table.ForeignKey(
                        name: "FK_Expenses_DateReferences_DateReferenceDateContainerId",
                        column: x => x.DateReferenceDateContainerId,
                        principalTable: "DateReferences",
                        principalColumn: "DateContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SalesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateReferenceDateContainerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BusinessName = table.Column<string>(type: "TEXT", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SalesId);
                    table.ForeignKey(
                        name: "FK_Sales_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentId");
                    table.ForeignKey(
                        name: "FK_Sales_DateReferences_DateReferenceDateContainerId",
                        column: x => x.DateReferenceDateContainerId,
                        principalTable: "DateReferences",
                        principalColumn: "DateContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractIncome",
                columns: table => new
                {
                    ContractualAgreementId = table.Column<Guid>(type: "TEXT", nullable: false),
                    InvoiceDateReferenceDateContainerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BusinessName = table.Column<string>(type: "TEXT", nullable: false),
                    ExpensesEntityExpensesId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SalesEntitySalesId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AttachmentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractIncome", x => x.ContractualAgreementId);
                    table.ForeignKey(
                        name: "FK_ContractIncome_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentId");
                    table.ForeignKey(
                        name: "FK_ContractIncome_DateReferences_InvoiceDateReferenceDateContainerId",
                        column: x => x.InvoiceDateReferenceDateContainerId,
                        principalTable: "DateReferences",
                        principalColumn: "DateContainerId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ContractIncome_AttachmentId",
                table: "ContractIncome",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_ExpensesEntityExpensesId",
                table: "ContractIncome",
                column: "ExpensesEntityExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_InvoiceDateReferenceDateContainerId",
                table: "ContractIncome",
                column: "InvoiceDateReferenceDateContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractIncome_SalesEntitySalesId",
                table: "ContractIncome",
                column: "SalesEntitySalesId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AttachmentId",
                table: "Expenses",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_DateReferenceDateContainerId",
                table: "Expenses",
                column: "DateReferenceDateContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_AttachmentId",
                table: "Sales",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_DateReferenceDateContainerId",
                table: "Sales",
                column: "DateReferenceDateContainerId");
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

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "DateReferences");
        }
    }
}
