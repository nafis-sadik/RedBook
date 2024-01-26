using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOrganizationCacheTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_OrganizationCache",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrganizationCache",
                table: "Products");

            migrationBuilder.DropTable(
                name: "OrganizationCache");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrganizationId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_OrganizationId",
                table: "Inventory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationCache",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCache", x => x.OrganizationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrganizationId",
                table: "Products",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_OrganizationId",
                table: "Inventory",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_OrganizationCache",
                table: "Inventory",
                column: "OrganizationId",
                principalTable: "OrganizationCache",
                principalColumn: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrganizationCache",
                table: "Products",
                column: "OrganizationId",
                principalTable: "OrganizationCache",
                principalColumn: "OrganizationId");
        }
    }
}
