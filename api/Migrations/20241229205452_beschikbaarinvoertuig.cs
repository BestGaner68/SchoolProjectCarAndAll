using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class beschikbaarinvoertuig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17eb2211-3030-4d6a-9b5c-0c0fefb0634a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97f5d3e9-339a-41e3-8a54-b8726acdd1df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5af3b7c-e0dc-4e96-bc03-c04e2bcd2687");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f845b46e-8358-4151-ae92-b1313080faf6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9017f10-cea1-4df3-82bc-169fc1b79001");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9aa9e0ec-9756-46c2-9d30-a11516653b33", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "bdfa37b4-9185-4f9a-bed2-2c7911cb7d1b", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "d1cb3be3-9c24-42f3-afa7-7d8cb7cdcf6c", null, "bedrijflant", "BEDRIJFKLANT" },
                    { "fab8ddb6-471e-4b88-9ca2-44ae76304ddb", null, "backendWorker", "BACKENDWORKER" },
                    { "fda72ad4-45b3-409a-92e5-1ea5266439df", null, "frontendWorker", "FRONTENDWORKER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aa9e0ec-9756-46c2-9d30-a11516653b33");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bdfa37b4-9185-4f9a-bed2-2c7911cb7d1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1cb3be3-9c24-42f3-afa7-7d8cb7cdcf6c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab8ddb6-471e-4b88-9ca2-44ae76304ddb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fda72ad4-45b3-409a-92e5-1ea5266439df");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17eb2211-3030-4d6a-9b5c-0c0fefb0634a", null, "frontendWorker", "FRONTENDWORKER" },
                    { "97f5d3e9-339a-41e3-8a54-b8726acdd1df", null, "backendWorker", "BACKENDWORKER" },
                    { "c5af3b7c-e0dc-4e96-bc03-c04e2bcd2687", null, "bedrijflant", "BEDRIJFKLANT" },
                    { "f845b46e-8358-4151-ae92-b1313080faf6", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "f9017f10-cea1-4df3-82bc-169fc1b79001", null, "particuliereKlant", "PARTICULIEREKLANT" }
                });
        }
    }
}
