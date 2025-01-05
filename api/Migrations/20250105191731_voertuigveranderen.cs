using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class voertuigveranderen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Beschikbaar",
                table: "Voertuig");

            // Drop the primary key constraint first
            migrationBuilder.DropPrimaryKey(
                name: "PK_Voertuig",
                table: "Voertuig");

            // Drop the old VoertuigId column
            migrationBuilder.DropColumn(
                name: "VoertuigId",
                table: "Voertuig");

            // Recreate the VoertuigId column with the IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "VoertuigId",
                table: "Voertuig",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Re-add the primary key constraint on the new VoertuigId column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Voertuig",
                table: "Voertuig",
                column: "VoertuigId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Voertuig",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "kleur",
                table: "Voertuig",
                newName: "Kleur");

            migrationBuilder.AddColumn<string>(
                name: "Soort",
                table: "Voertuig",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Soort",
                table: "Voertuig");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Voertuig",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Kleur",
                table: "Voertuig",
                newName: "kleur");

            migrationBuilder.AlterColumn<string>(
                name: "VoertuigId",
                table: "Voertuig",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Beschikbaar",
                table: "Voertuig",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
