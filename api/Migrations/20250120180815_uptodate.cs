using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class uptodate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BedrijfsString",
                table: "Wagenpark");

            migrationBuilder.RenameColumn(
                name: "NieuwWagenParkVerzoekId",
                table: "NieuwWagenParkVerzoek",
                newName: "WagenparkVerzoekId");

            migrationBuilder.AddColumn<string>(
                name: "Achternaam",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bedrijfsnaam",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GewensdeUsername",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KvkNummer",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Voornaam",
                table: "NieuwWagenParkVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achternaam",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.DropColumn(
                name: "Bedrijfsnaam",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.DropColumn(
                name: "GewensdeUsername",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.DropColumn(
                name: "KvkNummer",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.DropColumn(
                name: "Voornaam",
                table: "NieuwWagenParkVerzoek");

            migrationBuilder.RenameColumn(
                name: "WagenparkVerzoekId",
                table: "NieuwWagenParkVerzoek",
                newName: "NieuwWagenParkVerzoekId");

            migrationBuilder.AddColumn<string>(
                name: "BedrijfsString",
                table: "Wagenpark",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
