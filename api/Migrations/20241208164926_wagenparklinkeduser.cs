using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class wagenparklinkeduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b1a004e-2486-4626-a28a-c87dd67f51dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9c6c00c-01b7-4d89-9bd9-89865c14b949");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c925d03d-e77e-4795-a9a8-a62c2b56d5c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d098a21a-409d-40e3-b078-ff9ed902e67c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f23f6d89-38a5-46a4-87c8-8af0011ab65c");

            migrationBuilder.CreateTable(
                name: "wagenparkUserLinked",
                columns: table => new
                {
                    WagenparkId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wagenparkUserLinked", x => new { x.WagenparkId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_wagenparkUserLinked_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_wagenparkUserLinked_wagenPark_WagenparkId",
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
                    { "0bd5e5d8-9d35-48a1-b616-d0fb9817c3ba", null, "frontendWorker", "FRONTENDWORKER" },
                    { "4901d625-2415-4f16-a44f-4c4561c7d8ce", null, "zakelijkeKlant", "BEDRIJFKLANT" },
                    { "f7994ad4-4ae1-4a74-b507-11dd1e3e8df8", null, "backendWorker", "BACKENDWORKER" },
                    { "f82f71cd-373f-4e4f-87be-cdf1db1dbb7d", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "fa104bd1-2f30-4cfc-8d29-dcb00a4eb4cf", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_wagenparkUserLinked_AppUserId",
                table: "wagenparkUserLinked",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wagenparkUserLinked");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "481cdf0d-3713-48e5-ba44-42aee0785288", null, "frontendWorker", "FRONTENDWORKER" },
                    { "56c264eb-5fcb-4c62-bbfe-52004588a97a", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "beae6ccd-74b4-4d0a-8174-91e74dac21cb", null, "backendWorker", "BACKENDWORKER" },
                    { "d915dd04-b889-4934-a11c-f486f2131dbf", null, "zakelijkeKlant", "BEDRIJFKLANT" },
                    { "f0ce24c0-ed64-4726-928f-bdb1c95f4082", null, "particuliereKlant", "PARTICULIEREKLANT" }
                });
        }
    }
}
