using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Domain.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Purchase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 16, 9, 15, 982, DateTimeKind.Utc).AddTicks(3655));

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 16, 9, 15, 982, DateTimeKind.Utc).AddTicks(3661));

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 16, 9, 15, 982, DateTimeKind.Utc).AddTicks(3663));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Inventory");

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "CommonAttribute",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2023, 2, 18, 13, 38, 52, 630, DateTimeKind.Utc).AddTicks(9570));
        }
    }
}
