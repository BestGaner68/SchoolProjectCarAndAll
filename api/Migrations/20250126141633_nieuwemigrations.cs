using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class nieuwemigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VoertuigData",
                table: "VoertuigData");

            migrationBuilder.AddColumn<int>(
                name: "VoertuigDataId",
                table: "VoertuigData",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoertuigData",
                table: "VoertuigData",
                column: "VoertuigDataId");

            migrationBuilder.CreateIndex(
                name: "IX_VoertuigData_VoertuigId",
                table: "VoertuigData",
                column: "VoertuigId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VoertuigData",
                table: "VoertuigData");

            migrationBuilder.DropIndex(
                name: "IX_VoertuigData_VoertuigId",
                table: "VoertuigData");

            migrationBuilder.DropColumn(
                name: "VoertuigDataId",
                table: "VoertuigData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoertuigData",
                table: "VoertuigData",
                column: "VoertuigId");
        }
    }
}
