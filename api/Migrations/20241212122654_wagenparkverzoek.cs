using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class wagenparkverzoek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bd5e5d8-9d35-48a1-b616-d0fb9817c3ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4901d625-2415-4f16-a44f-4c4561c7d8ce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7994ad4-4ae1-4a74-b507-11dd1e3e8df8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f82f71cd-373f-4e4f-87be-cdf1db1dbb7d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa104bd1-2f30-4cfc-8d29-dcb00a4eb4cf");

            migrationBuilder.CreateTable(
                name: "wagenparkVerzoeken",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WagenparkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wagenparkVerzoeken", x => new { x.WagenparkId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_wagenparkVerzoeken_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_wagenparkVerzoeken_wagenPark_WagenparkId",
                        column: x => x.WagenparkId,
                        principalTable: "wagenPark",
                        principalColumn: "WagenParkId",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_wagenparkVerzoeken_AppUserId",
                table: "wagenparkVerzoeken",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wagenparkVerzoeken");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0bd5e5d8-9d35-48a1-b616-d0fb9817c3ba", null, "frontendWorker", "FRONTENDWORKER" },
                    { "4901d625-2415-4f16-a44f-4c4561c7d8ce", null, "zakelijkeKlant", "BEDRIJFKLANT" },
                    { "f7994ad4-4ae1-4a74-b507-11dd1e3e8df8", null, "backendWorker", "BACKENDWORKER" },
                    { "f82f71cd-373f-4e4f-87be-cdf1db1dbb7d", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "fa104bd1-2f30-4cfc-8d29-dcb00a4eb4cf", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" }
                });
        }
    }
}
