using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class wagenpark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ac243c3-da9a-4f99-884c-8003f8b730be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "860e78b2-f35b-4e4a-8ac5-72e80bd26cce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0e38b07-ac7b-4697-a196-2ea4da8a2c6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad1e2cbb-5a06-4a1a-917f-68c45a50e900");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9aeb37c-43d7-443a-8406-737f9461109b");

            migrationBuilder.CreateTable(
                name: "wagenPark",
                columns: table => new
                {
                    WagenParkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Bedrijfsnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BedrijfsString = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wagenPark", x => x.WagenParkId);
                    table.ForeignKey(
                        name: "FK_wagenPark_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_wagenPark_AppUserId",
                table: "wagenPark",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wagenPark");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "481cdf0d-3713-48e5-ba44-42aee0785288");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56c264eb-5fcb-4c62-bbfe-52004588a97a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "beae6ccd-74b4-4d0a-8174-91e74dac21cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d915dd04-b889-4934-a11c-f486f2131dbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0ce24c0-ed64-4726-928f-bdb1c95f4082");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6ac243c3-da9a-4f99-884c-8003f8b730be", null, "zakelijkeKlant", "BEDRIJFKLANT" },
                    { "860e78b2-f35b-4e4a-8ac5-72e80bd26cce", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "a0e38b07-ac7b-4697-a196-2ea4da8a2c6e", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "ad1e2cbb-5a06-4a1a-917f-68c45a50e900", null, "frontendWorker", "FRONTENDWORKER" },
                    { "c9aeb37c-43d7-443a-8406-737f9461109b", null, "backendWorker", "BACKENDWORKER" }
                });
        }
    }
}
