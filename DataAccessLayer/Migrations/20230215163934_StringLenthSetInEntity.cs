using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Domain.Migrations
{
    /// <inheritdoc />
    public partial class StringLenthSetInEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CheckNumber",
                table: "Purchases",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CheckNumber",
                table: "Purchases",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 36, 14, 154, DateTimeKind.Utc).AddTicks(4064));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 36, 14, 154, DateTimeKind.Utc).AddTicks(4068));

            migrationBuilder.UpdateData(
                table: "CommonAttributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2023, 2, 15, 16, 36, 14, 154, DateTimeKind.Utc).AddTicks(4070));
        }
    }
}
