using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class camperEnCaravan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "voertuigIdId",
                table: "Auto",
                newName: "AutoId");

            migrationBuilder.CreateTable(
                name: "camper",
                columns: table => new
                {
                    CamperId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_camper", x => x.CamperId);
                });

            migrationBuilder.CreateTable(
                name: "caravan",
                columns: table => new
                {
                    CaravanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caravan", x => x.CaravanId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2284a104-f64d-4b63-ba4a-b68d6a05125e", null, "wagenparkBeheerder", "WAGENPARKBEHEERDER" },
                    { "3d097945-4c84-457c-93dd-931ac5646c7d", null, "particuliereKlant", "PARTICULIEREKLANT" },
                    { "93eda2a1-ec68-4b5f-bd11-7d8fbe944653", null, "frontendWorker", "FRONTENDWORKER" },
                    { "bb8d7dc2-fbc1-4731-96d8-cc5a06768bfd", null, "backendWorker", "BACKENDWORKER" },
                    { "f1541ea5-c7aa-44f7-8cbe-e30c50c522fa", null, "bedrijflant", "BEDRIJFKLANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "camper");

            migrationBuilder.DropTable(
                name: "caravan");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2284a104-f64d-4b63-ba4a-b68d6a05125e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d097945-4c84-457c-93dd-931ac5646c7d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93eda2a1-ec68-4b5f-bd11-7d8fbe944653");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb8d7dc2-fbc1-4731-96d8-cc5a06768bfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1541ea5-c7aa-44f7-8cbe-e30c50c522fa");

            migrationBuilder.RenameColumn(
                name: "AutoId",
                table: "Auto",
                newName: "voertuigIdId");

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
        }
    }
}
