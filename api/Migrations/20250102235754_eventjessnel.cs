using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class eventjessnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "VerhuurVerzoek");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "VerhuurVerzoek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "VerhuurVerzoek");

            migrationBuilder.AddColumn<int>(
                name: "KlantId",
                table: "VerhuurVerzoek",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
