using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Gameplay.Database.Migrations
{
    public partial class AddFieldsToAccountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "LatestConnectedWorld",
                table: "Accounts",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PIN",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SPW",
                table: "Accounts",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestConnectedWorld",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PIN",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SPW",
                table: "Accounts");
        }
    }
}
