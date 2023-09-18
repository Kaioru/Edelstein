using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrationModelsToUseJsonColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Character",
                table: "migrations");
            migrationBuilder.DropColumn(
                name: "AccountWorld",
                table: "migrations");
            migrationBuilder.DropColumn(
                name: "Account",
                table: "migrations");
            
            migrationBuilder.AddColumn<string>(
                name: "Character",
                table: "migrations",
                type: "json",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AccountWorld",
                table: "migrations",
                type: "json",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "migrations",
                type: "json",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Character",
                table: "migrations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json");

            migrationBuilder.AlterColumn<string>(
                name: "AccountWorld",
                table: "migrations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json");

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "migrations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json");
        }
    }
}
