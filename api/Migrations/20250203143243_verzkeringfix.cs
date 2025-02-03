using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class verzkeringfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservering_Verzekering_VerzekeringId",
                table: "Reservering");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurVerzoek_Verzekering_VerzekeringId",
                table: "VerhuurVerzoek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verzekering",
                table: "Verzekering");

            migrationBuilder.RenameTable(
                name: "Verzekering",
                newName: "Verzekeringen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verzekeringen",
                table: "Verzekeringen",
                column: "VerzekeringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservering_Verzekeringen_VerzekeringId",
                table: "Reservering",
                column: "VerzekeringId",
                principalTable: "Verzekeringen",
                principalColumn: "VerzekeringId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurVerzoek_Verzekeringen_VerzekeringId",
                table: "VerhuurVerzoek",
                column: "VerzekeringId",
                principalTable: "Verzekeringen",
                principalColumn: "VerzekeringId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservering_Verzekeringen_VerzekeringId",
                table: "Reservering");

            migrationBuilder.DropForeignKey(
                name: "FK_VerhuurVerzoek_Verzekeringen_VerzekeringId",
                table: "VerhuurVerzoek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verzekeringen",
                table: "Verzekeringen");

            migrationBuilder.RenameTable(
                name: "Verzekeringen",
                newName: "Verzekering");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verzekering",
                table: "Verzekering",
                column: "VerzekeringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservering_Verzekering_VerzekeringId",
                table: "Reservering",
                column: "VerzekeringId",
                principalTable: "Verzekering",
                principalColumn: "VerzekeringId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerhuurVerzoek_Verzekering_VerzekeringId",
                table: "VerhuurVerzoek",
                column: "VerzekeringId",
                principalTable: "Verzekering",
                principalColumn: "VerzekeringId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
