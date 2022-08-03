using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class AddAccountIDToMigrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Migrations",
                table: "Migrations");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Migrations",
                newName: "AccountID");

            migrationBuilder.AlterColumn<int>(
                name: "AccountID",
                table: "Migrations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CharacterID",
                table: "Migrations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Migrations",
                table: "Migrations",
                column: "CharacterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Migrations",
                table: "Migrations");

            migrationBuilder.DropColumn(
                name: "CharacterID",
                table: "Migrations");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Migrations",
                newName: "ID");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Migrations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Migrations",
                table: "Migrations",
                column: "ID");
        }
    }
}
