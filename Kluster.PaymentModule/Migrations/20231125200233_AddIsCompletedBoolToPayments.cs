using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.PaymentModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedBoolToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "PaymentModule",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "PaymentModule",
                table: "Payments");
        }
    }
}
