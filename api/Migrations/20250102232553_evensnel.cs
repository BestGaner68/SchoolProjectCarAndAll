using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class evensnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wagenPark_AspNetUsers_AppUserId",
                table: "wagenPark");

            migrationBuilder.DropForeignKey(
                name: "FK_wagenparkUserLinked_AspNetUsers_AppUserId",
                table: "wagenparkUserLinked");

            migrationBuilder.DropForeignKey(
                name: "FK_wagenparkUserLinked_wagenPark_WagenparkId",
                table: "wagenparkUserLinked");

            migrationBuilder.DropForeignKey(
                name: "FK_wagenparkVerzoeken_AspNetUsers_AppUserId",
                table: "wagenparkVerzoeken");

            migrationBuilder.DropForeignKey(
                name: "FK_wagenparkVerzoeken_wagenPark_WagenparkId",
                table: "wagenparkVerzoeken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wagenparkUserLinked",
                table: "wagenparkUserLinked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wagenPark",
                table: "wagenPark");

            migrationBuilder.DropPrimaryKey(
                name: "PK_voertuigStatus",
                table: "voertuigStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_verhuurVerzoek",
                table: "verhuurVerzoek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reservering",
                table: "reservering");

            migrationBuilder.DropPrimaryKey(
                name: "PK_caravan",
                table: "caravan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_camper",
                table: "camper");

            migrationBuilder.RenameTable(
                name: "wagenparkVerzoeken",
                newName: "WagenparkVerzoeken");

            migrationBuilder.RenameTable(
                name: "wagenparkUserLinked",
                newName: "WagenparkUserLinked");

            migrationBuilder.RenameTable(
                name: "wagenPark",
                newName: "Wagenpark");

            migrationBuilder.RenameTable(
                name: "voertuigStatus",
                newName: "VoertuigStatus");

            migrationBuilder.RenameTable(
                name: "verhuurVerzoek",
                newName: "VerhuurVerzoek");

            migrationBuilder.RenameTable(
                name: "reservering",
                newName: "Reservering");

            migrationBuilder.RenameTable(
                name: "caravan",
                newName: "Caravan");

            migrationBuilder.RenameTable(
                name: "camper",
                newName: "Camper");

            migrationBuilder.RenameIndex(
                name: "IX_wagenparkVerzoeken_WagenparkId",
                table: "WagenparkVerzoeken",
                newName: "IX_WagenparkVerzoeken_WagenparkId");

            migrationBuilder.RenameIndex(
                name: "IX_wagenparkVerzoeken_AppUserId",
                table: "WagenparkVerzoeken",
                newName: "IX_WagenparkVerzoeken_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_wagenparkUserLinked_AppUserId",
                table: "WagenparkUserLinked",
                newName: "IX_WagenparkUserLinked_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_wagenPark_AppUserId",
                table: "Wagenpark",
                newName: "IX_Wagenpark_AppUserId");

            migrationBuilder.DropColumn(
                name: "VoertuigId",
                table: "VoertuigStatus");

            migrationBuilder.AddColumn<int>(
                name: "VoertuigId",
                table: "VoertuigStatus",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropColumn(
                name: "ReserveringId",
                table: "Reservering");

            migrationBuilder.AddColumn<int>(
                name: "ReserveringId",
                table: "Reservering",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WagenparkVerzoeken",
                table: "WagenparkVerzoeken",
                column: "wagenparkverzoekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WagenparkUserLinked",
                table: "WagenparkUserLinked",
                columns: new[] { "WagenparkId", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wagenpark",
                table: "Wagenpark",
                column: "WagenParkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoertuigStatus",
                table: "VoertuigStatus",
                column: "VoertuigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerhuurVerzoek",
                table: "VerhuurVerzoek",
                column: "VerhuurVerzoekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservering",
                table: "Reservering",
                column: "ReserveringId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Caravan",
                table: "Caravan",
                column: "CaravanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Camper",
                table: "Camper",
                column: "CamperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wagenpark_AspNetUsers_AppUserId",
                table: "Wagenpark",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WagenparkUserLinked_AspNetUsers_AppUserId",
                table: "WagenparkUserLinked",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WagenparkUserLinked_Wagenpark_WagenparkId",
                table: "WagenparkUserLinked",
                column: "WagenparkId",
                principalTable: "Wagenpark",
                principalColumn: "WagenParkId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WagenparkVerzoeken_AspNetUsers_AppUserId",
                table: "WagenparkVerzoeken",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WagenparkVerzoeken_Wagenpark_WagenparkId",
                table: "WagenparkVerzoeken",
                column: "WagenparkId",
                principalTable: "Wagenpark",
                principalColumn: "WagenParkId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wagenpark_AspNetUsers_AppUserId",
                table: "Wagenpark");

            migrationBuilder.DropForeignKey(
                name: "FK_WagenparkUserLinked_AspNetUsers_AppUserId",
                table: "WagenparkUserLinked");

            migrationBuilder.DropForeignKey(
                name: "FK_WagenparkUserLinked_Wagenpark_WagenparkId",
                table: "WagenparkUserLinked");

            migrationBuilder.DropForeignKey(
                name: "FK_WagenparkVerzoeken_AspNetUsers_AppUserId",
                table: "WagenparkVerzoeken");

            migrationBuilder.DropForeignKey(
                name: "FK_WagenparkVerzoeken_Wagenpark_WagenparkId",
                table: "WagenparkVerzoeken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WagenparkVerzoeken",
                table: "WagenparkVerzoeken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WagenparkUserLinked",
                table: "WagenparkUserLinked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wagenpark",
                table: "Wagenpark");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoertuigStatus",
                table: "VoertuigStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerhuurVerzoek",
                table: "VerhuurVerzoek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservering",
                table: "Reservering");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Caravan",
                table: "Caravan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Camper",
                table: "Camper");

            migrationBuilder.RenameTable(
                name: "WagenparkVerzoeken",
                newName: "wagenparkVerzoeken");

            migrationBuilder.RenameTable(
                name: "WagenparkUserLinked",
                newName: "wagenparkUserLinked");

            migrationBuilder.RenameTable(
                name: "Wagenpark",
                newName: "wagenPark");

            migrationBuilder.RenameTable(
                name: "VoertuigStatus",
                newName: "voertuigStatus");

            migrationBuilder.RenameTable(
                name: "VerhuurVerzoek",
                newName: "verhuurVerzoek");

            migrationBuilder.RenameTable(
                name: "Reservering",
                newName: "reservering");

            migrationBuilder.RenameTable(
                name: "Caravan",
                newName: "caravan");

            migrationBuilder.RenameTable(
                name: "Camper",
                newName: "camper");

            migrationBuilder.RenameIndex(
                name: "IX_WagenparkVerzoeken_WagenparkId",
                table: "wagenparkVerzoeken",
                newName: "IX_wagenparkVerzoeken_WagenparkId");

            migrationBuilder.RenameIndex(
                name: "IX_WagenparkVerzoeken_AppUserId",
                table: "wagenparkVerzoeken",
                newName: "IX_wagenparkVerzoeken_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WagenparkUserLinked_AppUserId",
                table: "wagenparkUserLinked",
                newName: "IX_wagenparkUserLinked_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Wagenpark_AppUserId",
                table: "wagenPark",
                newName: "IX_wagenPark_AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "VoertuigId",
                table: "voertuigStatus",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "ReserveringId",
                table: "reservering",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wagenparkVerzoeken",
                table: "wagenparkVerzoeken",
                column: "wagenparkverzoekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wagenparkUserLinked",
                table: "wagenparkUserLinked",
                columns: new[] { "WagenparkId", "AppUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_wagenPark",
                table: "wagenPark",
                column: "WagenParkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_voertuigStatus",
                table: "voertuigStatus",
                column: "VoertuigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_verhuurVerzoek",
                table: "verhuurVerzoek",
                column: "VerhuurVerzoekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reservering",
                table: "reservering",
                column: "ReserveringId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_caravan",
                table: "caravan",
                column: "CaravanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_camper",
                table: "camper",
                column: "CamperId");

            migrationBuilder.AddForeignKey(
                name: "FK_wagenPark_AspNetUsers_AppUserId",
                table: "wagenPark",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wagenparkUserLinked_AspNetUsers_AppUserId",
                table: "wagenparkUserLinked",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wagenparkUserLinked_wagenPark_WagenparkId",
                table: "wagenparkUserLinked",
                column: "WagenparkId",
                principalTable: "wagenPark",
                principalColumn: "WagenParkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wagenparkVerzoeken_AspNetUsers_AppUserId",
                table: "wagenparkVerzoeken",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wagenparkVerzoeken_wagenPark_WagenparkId",
                table: "wagenparkVerzoeken",
                column: "WagenparkId",
                principalTable: "wagenPark",
                principalColumn: "WagenParkId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
