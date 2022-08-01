using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Gameplay.Database.Migrations
{
    public partial class RemoveVersionFromDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AccountWorlds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "AccountWorlds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
