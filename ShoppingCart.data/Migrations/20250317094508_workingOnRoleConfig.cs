using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingCart.data.Migrations
{
    /// <inheritdoc />
    public partial class workingOnRoleConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26bbf6ce-b693-44c1-8dd1-f1cc5cea92c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6cec149-9e81-4eb1-aafb-32875f818012");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7762bd0e-f400-45e7-a98c-de704d58c817", null, "Admin", "ADMIN" },
                    { "e61380b2-33c4-407d-af79-f72d9e89f111", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "26bbf6ce-b693-44c1-8dd1-f1cc5cea92c8", null, "Admin", "ADMIN" },
                    { "b6cec149-9e81-4eb1-aafb-32875f818012", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
