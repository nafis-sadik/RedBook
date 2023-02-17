using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Inventory.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankBranches_Purchases_ToBankBranchId",
                table: "BankBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CommonAttributes_AttributesId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "BankList");

            migrationBuilder.DropIndex(
                name: "IX_Products_AttributesId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_BankBranches_ToBankBranchId",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "AttributesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ToBankBranchId",
                table: "BankBranches");

            migrationBuilder.RenameColumn(
                name: "QuantityAttribute",
                table: "Products",
                newName: "QuantityAttributeId");

            migrationBuilder.AddColumn<int>(
                name: "SalesId",
                table: "SalesPaymentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalesId",
                table: "SalesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PurchasePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PurchaseDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuantityAttributeId",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "BankBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 2, 17, 17, 14, 41, 573, DateTimeKind.Utc).AddTicks(2586));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2023, 2, 17, 17, 14, 41, 573, DateTimeKind.Utc).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2023, 2, 17, 17, 14, 41, 573, DateTimeKind.Utc).AddTicks(2593));

            migrationBuilder.CreateIndex(
                name: "IX_SalesPaymentRecords_SalesId",
                table: "SalesPaymentRecords",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_SalesId",
                table: "SalesDetails",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_FromBankBranchId",
                table: "Purchases",
                column: "FromBankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_FromBankId",
                table: "Purchases",
                column: "FromBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ToBankBranchId",
                table: "Purchases",
                column: "ToBankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ToBankId",
                table: "Purchases",
                column: "ToBankId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_PurchaseId",
                table: "PurchasePayments",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PurchaseId",
                table: "PurchaseDetails",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_QuantityAttributeId",
                table: "Products",
                column: "QuantityAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_QuantityAttributeId",
                table: "Inventory",
                column: "QuantityAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranches_BankId",
                table: "BankBranches",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankBranches_Bank_BankId",
                table: "BankBranches",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_CommonAttributes_QuantityAttributeId",
                table: "Inventory",
                column: "QuantityAttributeId",
                principalTable: "CommonAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CommonAttributes_QuantityAttributeId",
                table: "Products",
                column: "QuantityAttributeId",
                principalTable: "CommonAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Purchases_PurchaseId",
                table: "PurchaseDetails",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayments_Purchases_PurchaseId",
                table: "PurchasePayments",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_BankBranches_FromBankBranchId",
                table: "Purchases",
                column: "FromBankBranchId",
                principalTable: "BankBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_BankBranches_ToBankBranchId",
                table: "Purchases",
                column: "ToBankBranchId",
                principalTable: "BankBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Bank_FromBankId",
                table: "Purchases",
                column: "FromBankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Bank_ToBankId",
                table: "Purchases",
                column: "ToBankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_Sales_SalesId",
                table: "SalesDetails",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPaymentRecords_Sales_SalesId",
                table: "SalesPaymentRecords",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankBranches_Bank_BankId",
                table: "BankBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_CommonAttributes_QuantityAttributeId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CommonAttributes_QuantityAttributeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Purchases_PurchaseId",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayments_Purchases_PurchaseId",
                table: "PurchasePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_BankBranches_FromBankBranchId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_BankBranches_ToBankBranchId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Bank_FromBankId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Bank_ToBankId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_Sales_SalesId",
                table: "SalesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPaymentRecords_Sales_SalesId",
                table: "SalesPaymentRecords");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropIndex(
                name: "IX_SalesPaymentRecords_SalesId",
                table: "SalesPaymentRecords");

            migrationBuilder.DropIndex(
                name: "IX_SalesDetails_SalesId",
                table: "SalesDetails");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_FromBankBranchId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_FromBankId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ToBankBranchId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ToBankId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePayments_PurchaseId",
                table: "PurchasePayments");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseDetails_PurchaseId",
                table: "PurchaseDetails");

            migrationBuilder.DropIndex(
                name: "IX_Products_QuantityAttributeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_QuantityAttributeId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_BankBranches_BankId",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "SalesPaymentRecords");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PurchasePayments");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "QuantityAttributeId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "BankBranches");

            migrationBuilder.RenameColumn(
                name: "QuantityAttributeId",
                table: "Products",
                newName: "QuantityAttribute");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "AttributesId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "ToBankBranchId",
                table: "BankBranches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ToBankId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankList_Purchases_ToBankId",
                        column: x => x.ToBankId,
                        principalTable: "Purchases",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 39, 33, 987, DateTimeKind.Utc).AddTicks(662));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 39, 33, 987, DateTimeKind.Utc).AddTicks(666));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 39, 33, 987, DateTimeKind.Utc).AddTicks(667));

            migrationBuilder.CreateIndex(
                name: "IX_Products_AttributesId",
                table: "Products",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranches_ToBankBranchId",
                table: "BankBranches",
                column: "ToBankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BankList_ToBankId",
                table: "BankList",
                column: "ToBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankBranches_Purchases_ToBankBranchId",
                table: "BankBranches",
                column: "ToBankBranchId",
                principalTable: "Purchases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CommonAttributes_AttributesId",
                table: "Products",
                column: "AttributesId",
                principalTable: "CommonAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
