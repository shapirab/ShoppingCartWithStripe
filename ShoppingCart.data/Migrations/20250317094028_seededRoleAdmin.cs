using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingCart.data.Migrations
{
    /// <inheritdoc />
    public partial class seededRoleAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26bbf6ce-b693-44c1-8dd1-f1cc5cea92c8", null, "Admin", "ADMIN" },
                    { "b6cec149-9e81-4eb1-aafb-32875f818012", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26bbf6ce-b693-44c1-8dd1-f1cc5cea92c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6cec149-9e81-4eb1-aafb-32875f818012");
        }
    }
}
