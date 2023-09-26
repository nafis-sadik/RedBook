using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRouteTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteTypeId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteTypesId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RouteTypes",
                columns: table => new
                {
                    RouteTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTypes", x => x.RouteTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_RouteTypes_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId",
                principalTable: "RouteTypes",
                principalColumn: "RouteTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_RouteTypes_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "RouteTypes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteTypeId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteTypesId",
                table: "Routes");
        }
    }
}
