using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class VariantsIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoiceDetails_Products_ProductId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoiceDetails_ProductId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "PurchaseInvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    VariantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariantName = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.VariantId);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceDetails_ProductVariantId",
                table: "PurchaseInvoiceDetails",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductId",
                table: "ProductVariant",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoiceDetails_ProductVariant_ProductVariantId",
                table: "PurchaseInvoiceDetails",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "VariantId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoiceDetails_ProductVariant_ProductVariantId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoiceDetails_ProductVariantId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "PurchaseInvoiceDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PurchaseInvoiceDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceDetails_ProductId",
                table: "PurchaseInvoiceDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoiceDetails_Products_ProductId",
                table: "PurchaseInvoiceDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
