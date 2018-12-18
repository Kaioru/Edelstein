using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddLatestServiceFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LatestConnectedService",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "LatestConnectedWorld",
                table: "Accounts",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "PreviousConnectedService",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestConnectedService",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LatestConnectedWorld",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PreviousConnectedService",
                table: "Accounts");
        }
    }
}
