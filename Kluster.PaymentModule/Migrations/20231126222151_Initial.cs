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
                    InvoiceNo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOfIssuance = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    InvoiceItems = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    BillingAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientEmailAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    BusinessId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
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
                    PaymentReference = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DateOfPayment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OtherDetails = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    PaymentChannel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    BusinessId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    InvoiceId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
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
