using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class Customersadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecievedBy",
                table: "SalesPaymentRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPaymentRecords_PaidBy",
                table: "SalesPaymentRecords",
                column: "PaidBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPaymentRecords_Customers_PaidBy",
                table: "SalesPaymentRecords",
                column: "PaidBy",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPaymentRecords_Customers_PaidBy",
                table: "SalesPaymentRecords");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_SalesPaymentRecords_PaidBy",
                table: "SalesPaymentRecords");

            migrationBuilder.DropColumn(
                name: "RecievedBy",
                table: "SalesPaymentRecords");
        }
    }
}
