using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillsAndExtendSPColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json");

            migrationBuilder.AddColumn<string>(
                name: "ExtendSP",
                table: "characters",
                type: "json",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "characters",
                type: "json",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtendSP",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "characters");

            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);
        }
    }
}
