using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class nieuw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accessoires_Reservering_ReserveringId",
                table: "Accessoires");

            migrationBuilder.DropForeignKey(
                name: "FK_Accessoires_VerhuurVerzoek_VerhuurVerzoekId",
                table: "Accessoires");

            migrationBuilder.DropIndex(
                name: "IX_Accessoires_ReserveringId",
                table: "Accessoires");

            migrationBuilder.DropIndex(
                name: "IX_Accessoires_VerhuurVerzoekId",
                table: "Accessoires");

            migrationBuilder.DropColumn(
                name: "ReserveringId",
                table: "Accessoires");

            migrationBuilder.DropColumn(
                name: "VerhuurVerzoekId",
                table: "Accessoires");

            migrationBuilder.CreateTable(
                name: "AccessoiresReservering",
                columns: table => new
                {
                    AccessoiresId = table.Column<int>(type: "int", nullable: false),
                    ReserveringId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoiresReservering", x => new { x.AccessoiresId, x.ReserveringId });
                    table.ForeignKey(
                        name: "FK_AccessoiresReservering_Accessoires_AccessoiresId",
                        column: x => x.AccessoiresId,
                        principalTable: "Accessoires",
                        principalColumn: "AccessoiresId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessoiresReservering_Reservering_ReserveringId",
                        column: x => x.ReserveringId,
                        principalTable: "Reservering",
                        principalColumn: "ReserveringId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessoiresVerhuurVerzoek",
                columns: table => new
                {
                    AccessoiresId = table.Column<int>(type: "int", nullable: false),
                    VerhuurVerzoekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoiresVerhuurVerzoek", x => new { x.AccessoiresId, x.VerhuurVerzoekId });
                    table.ForeignKey(
                        name: "FK_AccessoiresVerhuurVerzoek_Accessoires_AccessoiresId",
                        column: x => x.AccessoiresId,
                        principalTable: "Accessoires",
                        principalColumn: "AccessoiresId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessoiresVerhuurVerzoek_VerhuurVerzoek_VerhuurVerzoekId",
                        column: x => x.VerhuurVerzoekId,
                        principalTable: "VerhuurVerzoek",
                        principalColumn: "VerhuurVerzoekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessoiresReservering_ReserveringId",
                table: "AccessoiresReservering",
                column: "ReserveringId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoiresVerhuurVerzoek_VerhuurVerzoekId",
                table: "AccessoiresVerhuurVerzoek",
                column: "VerhuurVerzoekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessoiresReservering");

            migrationBuilder.DropTable(
                name: "AccessoiresVerhuurVerzoek");

            migrationBuilder.AddColumn<int>(
                name: "ReserveringId",
                table: "Accessoires",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerhuurVerzoekId",
                table: "Accessoires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accessoires_ReserveringId",
                table: "Accessoires",
                column: "ReserveringId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessoires_VerhuurVerzoekId",
                table: "Accessoires",
                column: "VerhuurVerzoekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accessoires_Reservering_ReserveringId",
                table: "Accessoires",
                column: "ReserveringId",
                principalTable: "Reservering",
                principalColumn: "ReserveringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accessoires_VerhuurVerzoek_VerhuurVerzoekId",
                table: "Accessoires",
                column: "VerhuurVerzoekId",
                principalTable: "VerhuurVerzoek",
                principalColumn: "VerhuurVerzoekId");
        }
    }
}
