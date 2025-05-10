using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductVariantNewColumnsIncludingSKU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductVariant",
                newName: "StockQuantity");

            migrationBuilder.AddColumn<int>(
                name: "ColorAttributeId",
                table: "ProductVariant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "ProductVariant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorAttributeId",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "ProductVariant");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "ProductVariant",
                newName: "Quantity");
        }
    }
}
