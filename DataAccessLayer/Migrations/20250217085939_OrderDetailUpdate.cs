using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderDetailUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "PurchaseInvoiceDetails",
                newName: "PurchaseDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxRetailDiscount",
                table: "PurchaseInvoiceDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductVariantName",
                table: "PurchaseInvoiceDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxRetailDiscount",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ProductVariantName",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "PurchaseDiscount",
                table: "PurchaseInvoiceDetails",
                newName: "Discount");
        }
    }
}
