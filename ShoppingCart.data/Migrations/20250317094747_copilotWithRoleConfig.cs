using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingCart.data.Migrations
{
    /// <inheritdoc />
    public partial class copilotWithRoleConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7762bd0e-f400-45e7-a98c-de704d58c817");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e61380b2-33c4-407d-af79-f72d9e89f111");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b8a9d9a1-d029-4f21-a9cd-b0df9e0f3abe", null, "Admin", "ADMIN" },
                    { "edc3c74c-e90e-45d7-a7c4-4c3c7c4b1234", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8a9d9a1-d029-4f21-a9cd-b0df9e0f3abe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edc3c74c-e90e-45d7-a7c4-4c3c7c4b1234");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7762bd0e-f400-45e7-a98c-de704d58c817", null, "Admin", "ADMIN" },
                    { "e61380b2-33c4-407d-af79-f72d9e89f111", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
