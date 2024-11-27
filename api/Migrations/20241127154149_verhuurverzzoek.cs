using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class verhuurverzzoek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Auto",
                newName: "voertuigIdId");

            migrationBuilder.CreateTable(
                name: "verhuurVerzoek",
                columns: table => new
                {
                    VerhuurVerzoekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoertuigId = table.Column<int>(type: "int", nullable: false),
                    KlantId = table.Column<int>(type: "int", nullable: false),
                    StartDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AardReis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bestemming = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerwachtteKM = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verhuurVerzoek", x => x.VerhuurVerzoekId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verhuurVerzoek");

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

            migrationBuilder.RenameColumn(
                name: "voertuigIdId",
                table: "Auto",
                newName: "Id");

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
    }
}
