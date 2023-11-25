using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.PaymentModule.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PaymentModule");

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "PaymentModule",
                columns: table => new
                {
                    InvoiceNo = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfIssuance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InvoiceItems = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    BillingAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    BusinessId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceNo);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "PaymentModule",
                columns: table => new
                {
                    PaymentReference = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateOfPayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OtherDetails = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    PaymentChannel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentReference);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "PaymentModule",
                        principalTable: "Invoices",
                        principalColumn: "InvoiceNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                schema: "PaymentModule",
                table: "Payments",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "PaymentModule");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "PaymentModule");
        }
    }
}
