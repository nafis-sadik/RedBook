using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "OrganizationName" },
                values: new object[] { 1, "Blume Digital Corp." });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "OrganizationId", "RoleName" },
                values: new object[] { 1, 1, "System Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountBalance", "FirstName", "LastName", "OrganizationId", "Password", "Status", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", 999999999999m, "Nafis", "Sadik", 1, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJPQk8xM25hZnUuIiwibmJmIjoxNjc3MDI0MDM2LCJleHAiOjE2Nzc2Mjg4MzYsImlhdCI6MTY3NzAyNDAzNn0.pmlESxWJkdBj8SRc_8AMksUIOFQM0T2rhsuN7B54QQ4", "A", "nafis_sadik" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
