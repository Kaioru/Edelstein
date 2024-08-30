using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationEntitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountSnapshot",
                table: "migration_info",
                type: "json",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountWorldDataSnapshot",
                table: "migration_info",
                type: "json",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CharacterSnapshot",
                table: "migration_info",
                type: "json",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountSnapshot",
                table: "migration_info");

            migrationBuilder.DropColumn(
                name: "AccountWorldDataSnapshot",
                table: "migration_info");

            migrationBuilder.DropColumn(
                name: "CharacterSnapshot",
                table: "migration_info");
        }
    }
}
