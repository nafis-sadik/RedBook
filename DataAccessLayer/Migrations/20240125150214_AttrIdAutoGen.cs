using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttrIdAutoGen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CommonAttribute",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "CommonAttribute");

            migrationBuilder.AddColumn<int>(
                name: "AttributeId",
                table: "CommonAttribute",
                type: "int",
                nullable: false
            ).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CommonAttribute",
                table: "Products",
                column: "QuantityAttributeId",
                principalTable: "CommonAttribute",
                principalColumn: "AttributeId",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "YourTable");

            migrationBuilder.AddColumn<int>(
                name: "AttributeId",
                table: "CommonAttribute",
                type: "int",
                nullable: false).Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
