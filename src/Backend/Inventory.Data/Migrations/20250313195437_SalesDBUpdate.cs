using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class SalesDBUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPaymentRecords_Customers_PaidBy",
                table: "SalesPaymentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPaymentRecords_SalesInvoices_InvoiceId",
                table: "SalesPaymentRecords");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_SalesPaymentRecords_PaidBy",
                table: "SalesPaymentRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "ChalanNo",
                table: "SalesInvoices");

            migrationBuilder.RenameTable(
                name: "SalesInvoices",
                newName: "SalesInvoice");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "SalesInvoice",
                newName: "TotalDiscount");

            migrationBuilder.RenameColumn(
                name: "SoldBy",
                table: "SalesInvoice",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "SalesDate",
                table: "SalesInvoice",
                newName: "CreateDate");

            migrationBuilder.AddColumn<int>(
                name: "CreateBy",
                table: "SalesInvoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SalesInvoice",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InvoiceTotal",
                table: "SalesInvoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SalesInvoice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SalesInvoice",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Terms",
                table: "SalesInvoice",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesInvoice",
                table: "SalesInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateTable(
                name: "SalesInvoiceDetails",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductVariantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VariantId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoiceDetails", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceDetails_ProductVariant_VariantId",
                        column: x => x.VariantId,
                        principalTable: "ProductVariant",
                        principalColumn: "VariantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceDetails_PurchaseInvoices_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceDetails_SalesInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "SalesInvoice",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_CustomerId",
                table: "SalesInvoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceDetails_InvoiceId",
                table: "SalesInvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceDetails_PurchaseId",
                table: "SalesInvoiceDetails",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceDetails_VariantId",
                table: "SalesInvoiceDetails",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Customers_CustomerId",
                table: "SalesInvoice",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPaymentRecords_SalesInvoice_InvoiceId",
                table: "SalesPaymentRecords",
                column: "InvoiceId",
                principalTable: "SalesInvoice",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Customers_CustomerId",
                table: "SalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPaymentRecords_SalesInvoice_InvoiceId",
                table: "SalesPaymentRecords");

            migrationBuilder.DropTable(
                name: "SalesInvoiceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesInvoice",
                table: "SalesInvoice");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoice_CustomerId",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "InvoiceTotal",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "Terms",
                table: "SalesInvoice");

            migrationBuilder.RenameTable(
                name: "SalesInvoice",
                newName: "SalesInvoices");

            migrationBuilder.RenameColumn(
                name: "TotalDiscount",
                table: "SalesInvoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "SalesInvoices",
                newName: "SoldBy");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "SalesInvoices",
                newName: "SalesDate");

            migrationBuilder.AddColumn<string>(
                name: "ChalanNo",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesInvoices",
                table: "SalesInvoices",
                column: "InvoiceId");

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SalesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SalesId);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_PurchaseInvoices_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_SalesInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPaymentRecords_PaidBy",
                table: "SalesPaymentRecords",
                column: "PaidBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_InvoiceId",
                table: "Sales",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PurchaseId",
                table: "Sales",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPaymentRecords_Customers_PaidBy",
                table: "SalesPaymentRecords",
                column: "PaidBy",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPaymentRecords_SalesInvoices_InvoiceId",
                table: "SalesPaymentRecords",
                column: "InvoiceId",
                principalTable: "SalesInvoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
