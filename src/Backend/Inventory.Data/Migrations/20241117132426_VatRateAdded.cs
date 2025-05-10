using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class VatRateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VatRate",
                table: "PurchaseInvoiceDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "PurchaseInvoiceDetails");
        }
    }
}
