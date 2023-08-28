using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class SysAdminNRetailerAddedToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminRole",
                table: "Roles");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRetailer",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemAdmin",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsRetailer",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsSystemAdmin",
                table: "Roles");

            migrationBuilder.AddColumn<short>(
                name: "IsAdminRole",
                table: "Roles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
