using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class BrandIdNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Products_CommonAttributes_BrandId",
            //    table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Products_CommonAttributes_BrandId",
            //    table: "Products",
            //    column: "BrandId",
            //    principalTable: "CommonAttributes",
            //    principalColumn: "AttributeId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Products_CommonAttributes_BrandId",
            //    table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Products_CommonAttributes_BrandId",
            //    table: "Products",
            //    column: "BrandId",
            //    principalTable: "CommonAttributes",
            //    principalColumn: "AttributeId");
        }
    }
}
