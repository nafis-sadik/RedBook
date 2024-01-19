using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_UserCache",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_UserCache1",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserCache",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserCache1",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_UserCache",
                table: "SalesDetails");

            migrationBuilder.DropTable(
                name: "UserCache");

            migrationBuilder.DropIndex(
                name: "IX_SalesDetails_SoldBy",
                table: "SalesDetails");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreateBy",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UpdateBy",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UpdatedBy",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UpdateBy",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "UserCache",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCache", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_SoldBy",
                table: "SalesDetails",
                column: "SoldBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreateBy",
                table: "Products",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdateBy",
                table: "Products",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedBy",
                table: "Categories",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_UserCache",
                table: "Categories",
                column: "CreatedBy",
                principalTable: "UserCache",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_UserCache1",
                table: "Categories",
                column: "UpdatedBy",
                principalTable: "UserCache",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserCache",
                table: "Products",
                column: "UpdateBy",
                principalTable: "UserCache",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserCache1",
                table: "Products",
                column: "CreateBy",
                principalTable: "UserCache",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_UserCache",
                table: "SalesDetails",
                column: "SoldBy",
                principalTable: "UserCache",
                principalColumn: "UserId");
        }
    }
}
