using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActionAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCreateAccess",
                table: "RoleRouteMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasEditAccess",
                table: "RoleRouteMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasUpdateAccess",
                table: "RoleRouteMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCreateAccess",
                table: "RoleRouteMappings");

            migrationBuilder.DropColumn(
                name: "HasEditAccess",
                table: "RoleRouteMappings");

            migrationBuilder.DropColumn(
                name: "HasUpdateAccess",
                table: "RoleRouteMappings");
        }
    }
}
