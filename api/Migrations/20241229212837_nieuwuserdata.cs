using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class nieuwuserdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

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
    }
}
