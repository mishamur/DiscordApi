using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApi.Migrations
{
    public partial class compositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameStats_Users_UserPuuid",
                table: "PlayerGameStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerGameStats",
                table: "PlayerGameStats");

            migrationBuilder.AlterColumn<string>(
                name: "UserPuuid",
                table: "PlayerGameStats",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerGameStats",
                table: "PlayerGameStats",
                columns: new[] { "MatchId", "UserPuuid" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameStats_Users_UserPuuid",
                table: "PlayerGameStats",
                column: "UserPuuid",
                principalTable: "Users",
                principalColumn: "Puuid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameStats_Users_UserPuuid",
                table: "PlayerGameStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerGameStats",
                table: "PlayerGameStats");

            migrationBuilder.AlterColumn<string>(
                name: "UserPuuid",
                table: "PlayerGameStats",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerGameStats",
                table: "PlayerGameStats",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameStats_Users_UserPuuid",
                table: "PlayerGameStats",
                column: "UserPuuid",
                principalTable: "Users",
                principalColumn: "Puuid");
        }
    }
}
