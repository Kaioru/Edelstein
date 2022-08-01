using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class RemoveKeyFromMigrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Migrations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Migrations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
