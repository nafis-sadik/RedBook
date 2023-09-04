using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllowParentRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentRouteId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ParentRouteId",
                table: "Routes",
                column: "ParentRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Routes_ParentRouteId",
                table: "Routes",
                column: "ParentRouteId",
                principalTable: "Routes",
                principalColumn: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Routes_ParentRouteId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ParentRouteId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ParentRouteId",
                table: "Routes");
        }
    }
}
