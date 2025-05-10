using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class TermsNServiceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "PurchaseInvoices",
                type: "nvarchar(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChalanDate",
                table: "PurchaseInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Terms",
                table: "PurchaseInvoices",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChalanDate",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "Terms",
                table: "PurchaseInvoices");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)");
        }
    }
}
