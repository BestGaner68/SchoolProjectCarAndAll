using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Abonnenemtn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    AbonnementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbonnementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbonnementPrijs = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.AbonnementId);
                });

            migrationBuilder.CreateTable(
                name: "AbonnementUserLinkeds",
                columns: table => new
                {
                    AbonnementUserLinkedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbonnementId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbonnementUserLinkeds", x => x.AbonnementUserLinkedId);
                    table.ForeignKey(
                        name: "FK_AbonnementUserLinkeds_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "AbonnementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbonnementUserLinkeds_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbonnementUserLinkeds_AbonnementId",
                table: "AbonnementUserLinkeds",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_AbonnementUserLinkeds_AppUserId",
                table: "AbonnementUserLinkeds",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbonnementUserLinkeds");

            migrationBuilder.DropTable(
                name: "Abonnementen");
        }
    }
}
