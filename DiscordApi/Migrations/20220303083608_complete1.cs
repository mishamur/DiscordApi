using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApi.Migrations
{
    public partial class complete1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
