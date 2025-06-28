using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class SalesInvoiceDetails_PurchaseId_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceDetails_PurchaseInvoices_PurchaseId",
                table: "SalesInvoiceDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceDetails_PurchaseInvoiceDetails_PurchaseId",
                table: "SalesInvoiceDetails",
                column: "PurchaseId",
                principalTable: "PurchaseInvoiceDetails",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceDetails_PurchaseInvoiceDetails_PurchaseId",
                table: "SalesInvoiceDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceDetails_PurchaseInvoices_PurchaseId",
                table: "SalesInvoiceDetails",
                column: "PurchaseId",
                principalTable: "PurchaseInvoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
