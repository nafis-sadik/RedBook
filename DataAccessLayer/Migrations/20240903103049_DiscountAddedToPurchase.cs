using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class DiscountAddedToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseRecords");

            migrationBuilder.RenameColumn(
                name: "TotalPurchasePrice",
                table: "PurchaseInvoices",
                newName: "TotalDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossTotal",
                table: "PurchaseInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceDetails",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceDetails", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceDetails_PurchaseInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceDetails_InvoiceId",
                table: "PurchaseInvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceDetails_ProductId",
                table: "PurchaseInvoiceDetails",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "GrossTotal",
                table: "PurchaseInvoices");

            migrationBuilder.RenameColumn(
                name: "TotalDiscount",
                table: "PurchaseInvoices",
                newName: "TotalPurchasePrice");

            migrationBuilder.CreateTable(
                name: "PurchaseRecords",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductName = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRecords", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_PurchaseRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_PurchaseRecords_PurchaseInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRecords_InvoiceId",
                table: "PurchaseRecords",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRecords_ProductId",
                table: "PurchaseRecords",
                column: "ProductId");
        }
    }
}
