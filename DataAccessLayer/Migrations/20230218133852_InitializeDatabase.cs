using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CatagoryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ParentCategory = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catagories_Catagories",
                        column: x => x.ParentCategory,
                        principalTable: "Categories",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommonAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AttributeType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AttributeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonAttribute", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "longtext", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    QuantityAttributeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SalesDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    SoldBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BankBranch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    BranchName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankBranches_Banks",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalesPaymentRecords",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    MemoNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPaymentRecords", x => x.id);
                    table.ForeignKey(
                        name: "FK_SalesPaymentRecords_Sales",
                        column: x => x.MemoNumber,
                        principalTable: "Sales",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PurchasedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ToBankId = table.Column<int>(type: "int", nullable: false),
                    ToBankBranchId = table.Column<int>(type: "int", nullable: false),
                    FromBankId = table.Column<int>(type: "int", nullable: false),
                    FromBankBranchId = table.Column<int>(type: "int", nullable: false),
                    CheckNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_BankBranches",
                        column: x => x.ToBankBranchId,
                        principalTable: "BankBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_BankBranches1",
                        column: x => x.FromBankBranchId,
                        principalTable: "BankBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_Banks",
                        column: x => x.ToBankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchase_Banks1",
                        column: x => x.FromBankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    ChalanNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    QuantityAttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inventory_Purchase",
                        column: x => x.ChalanNumber,
                        principalTable: "Purchase",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ChalanNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Purchase1",
                        column: x => x.ChalanNumber,
                        principalTable: "Purchase",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchasePayments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    ChalanNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePayments", x => x.id);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentRecords_Purchase",
                        column: x => x.ChalanNumber,
                        principalTable: "Purchase",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalesDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    MemoNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ChalanNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_SalesDetails_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesDetails_Purchase",
                        column: x => x.ChalanNo,
                        principalTable: "Purchase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesDetails_Sales",
                        column: x => x.MemoNumber,
                        principalTable: "Sales",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "CommonAttribute",
                columns: new[] { "Id", "AttributeName", "AttributeType", "CreateDate", "CreatedBy", "UpdateBy", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Liter", "Quantity", new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9565), "SystemAdmin", "", null },
                    { 2, "Kg", "Quantity", new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9569), "SystemAdmin", "", null },
                    { 3, "Meter", "Quantity", new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9570), "SystemAdmin", "", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankBranch_BankId",
                table: "BankBranch",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategory",
                table: "Categories",
                column: "ParentCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ChalanNumber",
                table: "Inventory",
                column: "ChalanNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_FromBankBranchId",
                table: "Purchase",
                column: "FromBankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_FromBankId",
                table: "Purchase",
                column: "FromBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_ToBankBranchId",
                table: "Purchase",
                column: "ToBankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_ToBankId",
                table: "Purchase",
                column: "ToBankId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_ChalanNumber",
                table: "PurchaseDetails",
                column: "ChalanNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_ProductId",
                table: "PurchaseDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_ChalanNumber",
                table: "PurchasePayments",
                column: "ChalanNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_ChalanNo",
                table: "SalesDetails",
                column: "ChalanNo");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_MemoNumber",
                table: "SalesDetails",
                column: "MemoNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_ProductId",
                table: "SalesDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPaymentRecords_MemoNumber",
                table: "SalesPaymentRecords",
                column: "MemoNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CommonAttribute");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.DropTable(
                name: "PurchasePayments");

            migrationBuilder.DropTable(
                name: "SalesDetails");

            migrationBuilder.DropTable(
                name: "SalesPaymentRecords");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "BankBranch");

            migrationBuilder.DropTable(
                name: "Bank");
        }
    }
}
