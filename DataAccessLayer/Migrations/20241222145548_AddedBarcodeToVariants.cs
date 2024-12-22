using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBarcodeToVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorAttributeId",
                table: "ProductVariant");

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                table: "ProductVariant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "ProductVariant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "ProductVariant");

            migrationBuilder.AddColumn<int>(
                name: "ColorAttributeId",
                table: "ProductVariant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
