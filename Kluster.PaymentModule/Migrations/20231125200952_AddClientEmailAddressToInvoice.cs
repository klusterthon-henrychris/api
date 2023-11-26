using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.PaymentModule.Migrations
{
    /// <inheritdoc />
    public partial class AddClientEmailAddressToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientEmailAddress",
                schema: "PaymentModule",
                table: "Invoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientEmailAddress",
                schema: "PaymentModule",
                table: "Invoices");
        }
    }
}
