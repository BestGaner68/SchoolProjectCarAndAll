using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class registerseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03d1b59a-d1db-48fb-8175-974d7e08a745", null, "zakelijkeKlant", "BEDRIJFKLANT" },
                    { "0948e6ff-8b97-42a1-8bd4-42ba91e438dd", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "4c2b5a0e-7e89-441f-86df-27dcd5cdf94f", null, "frontendWorker", "FRONTENDWORKER" },
                    { "54cc5d4f-a048-4c9e-95f4-1bce8c84980d", null, "backendWorker", "BACKENDWORKER" },
                    { "f6473b67-b328-4a90-a362-7616e7664ed0", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03d1b59a-d1db-48fb-8175-974d7e08a745");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0948e6ff-8b97-42a1-8bd4-42ba91e438dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c2b5a0e-7e89-441f-86df-27dcd5cdf94f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54cc5d4f-a048-4c9e-95f4-1bce8c84980d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6473b67-b328-4a90-a362-7616e7664ed0");
        }
    }
}
