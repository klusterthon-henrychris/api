using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.BusinessModule.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCacNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CacNumber",
                table: "Businesses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CacNumber",
                table: "Businesses",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
