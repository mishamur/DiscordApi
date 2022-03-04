using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApi.Migrations
{
    public partial class TestUserWithPlayerGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AuthorId",
                table: "Users",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_AuthorId",
                table: "Users",
                column: "AuthorId");

            migrationBuilder.CreateTable(
                name: "PlayerGameStats",
                columns: table => new
                {
                    MatchId = table.Column<string>(type: "text", nullable: false),
                    GameStat_gameStartTimestamp = table.Column<string>(type: "text", nullable: true),
                    GameStat_gameEndTimestamp = table.Column<string>(type: "text", nullable: true),
                    PlayerStat_SummonerName = table.Column<string>(type: "text", nullable: true),
                    PlayerStat_Kills = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_Deaths = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_Assists = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_ChampionName = table.Column<string>(type: "text", nullable: true),
                    PlayerStat_ParticipantId = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_QuadraKills = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_PentaKills = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_TotalDamageDealtToChampions = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_Spell1Casts = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_Spell4Casts = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_VisionScore = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_Win = table.Column<bool>(type: "boolean", nullable: true),
                    PlayerStat_TotalMinionsKilled = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_NeutralMinionsKilled = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_TotalTimeSpentDead = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_WardsPlaced = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_GoldEarned = table.Column<int>(type: "integer", nullable: true),
                    PlayerStat_TimeCCingOthers = table.Column<int>(type: "integer", nullable: true),
                    UserPuuid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameStats", x => x.MatchId);
                    table.ForeignKey(
                        name: "FK_PlayerGameStats_Users_UserPuuid",
                        column: x => x.UserPuuid,
                        principalTable: "Users",
                        principalColumn: "Puuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameStats_UserPuuid",
                table: "PlayerGameStats",
                column: "UserPuuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameStats");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_AuthorId",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "AuthorId",
                table: "Users",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");
        }
    }
}
