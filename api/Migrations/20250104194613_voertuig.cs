using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class voertuig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auto");

            migrationBuilder.DropTable(
                name: "Camper");

            migrationBuilder.DropTable(
                name: "Caravan");

            migrationBuilder.CreateTable(
                name: "Voertuig",
                columns: table => new
                {
                    VoertuigId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false),
                    Beschikbaar = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuig", x => x.VoertuigId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voertuig");

            migrationBuilder.CreateTable(
                name: "Auto",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false),
                    Beschikbaar = table.Column<bool>(type: "bit", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto", x => x.AutoId);
                });

            migrationBuilder.CreateTable(
                name: "Camper",
                columns: table => new
                {
                    CamperId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false),
                    Beschikbaar = table.Column<bool>(type: "bit", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camper", x => x.CamperId);
                });

            migrationBuilder.CreateTable(
                name: "Caravan",
                columns: table => new
                {
                    CaravanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AanschafJaar = table.Column<int>(type: "int", nullable: false),
                    Beschikbaar = table.Column<bool>(type: "bit", nullable: false),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caravan", x => x.CaravanId);
                });
        }
    }
}
