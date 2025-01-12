using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class abonnementinwagenpark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AbonnementId",
                table: "Wagenpark",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wagenpark_AbonnementId",
                table: "Wagenpark",
                column: "AbonnementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wagenpark_Abonnementen_AbonnementId",
                table: "Wagenpark",
                column: "AbonnementId",
                principalTable: "Abonnementen",
                principalColumn: "AbonnementId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wagenpark_Abonnementen_AbonnementId",
                table: "Wagenpark");

            migrationBuilder.DropIndex(
                name: "IX_Wagenpark_AbonnementId",
                table: "Wagenpark");

            migrationBuilder.DropColumn(
                name: "AbonnementId",
                table: "Wagenpark");
        }
    }
}
