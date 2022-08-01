using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class AddKeyFromMigrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Key",
                table: "Migrations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Migrations");
        }
    }
}
