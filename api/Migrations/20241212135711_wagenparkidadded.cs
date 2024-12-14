using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class wagenparkidadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f74965d-5482-4cdf-b940-aa739fef931e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1c5589d-4ce9-437c-9eec-20f8125dca72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d205616c-eebf-4425-8dd2-1bac987c0c05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea32f859-0eb4-4be4-b1f5-e3907edbcb09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8af48f4-93b5-4630-be27-3e02f9d4fd6f");

            migrationBuilder.AddColumn<int>(
                name: "wagenparkverzoekId",
                table: "wagenparkVerzoeken",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken",
                column: "wagenparkverzoekId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f95ce73-c9f8-421c-8b0f-e23a95a6c0d8", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "36e6f772-6fdb-4d50-80bd-cfa0dab3707a", null, "frontendWorker", "FRONTENDWORKER" },
                    { "9c187ed0-8777-4390-b6fe-adffc804256c", null, "backendWorker", "BACKENDWORKER" },
                    { "ade36858-0055-4f91-aa02-0c8b9d80254e", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "f8d3d78f-0d9c-46f4-babc-75336ea71b40", null, "bedrijfKlant", "BEDRIJFKLANT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_wagenparkVerzoeken_WagenparkId",
                table: "wagenparkVerzoeken",
                column: "WagenparkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken");

            migrationBuilder.DropIndex(
                name: "IX_wagenparkVerzoeken_WagenparkId",
                table: "wagenparkVerzoeken");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f95ce73-c9f8-421c-8b0f-e23a95a6c0d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36e6f772-6fdb-4d50-80bd-cfa0dab3707a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c187ed0-8777-4390-b6fe-adffc804256c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ade36858-0055-4f91-aa02-0c8b9d80254e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8d3d78f-0d9c-46f4-babc-75336ea71b40");

            migrationBuilder.DropColumn(
                name: "wagenparkverzoekId",
                table: "wagenparkVerzoeken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken",
                columns: new[] { "WagenparkId", "AppUserId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f74965d-5482-4cdf-b940-aa739fef931e", null, "bedrijfKlant", "BEDRIJFKLANT" },
                    { "b1c5589d-4ce9-437c-9eec-20f8125dca72", null, "backendWorker", "BACKENDWORKER" },
                    { "d205616c-eebf-4425-8dd2-1bac987c0c05", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "ea32f859-0eb4-4be4-b1f5-e3907edbcb09", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "f8af48f4-93b5-4630-be27-3e02f9d4fd6f", null, "frontendWorker", "FRONTENDWORKER" }
                });
        }
    }
}
