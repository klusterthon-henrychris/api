using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.PaymentModule.Migrations
{
    /// <inheritdoc />
    public partial class MakeInvoiceItemsRequiredAndAddBillingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Client_ClientId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceItems",
                table: "Invoices",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Invoices",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Client_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceNo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Client_ClientId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceItems",
                table: "Invoices",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Client_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceNo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
