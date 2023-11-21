using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kluster.UserModule.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "AspNetUsers",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "UserType");
        }
    }
}
