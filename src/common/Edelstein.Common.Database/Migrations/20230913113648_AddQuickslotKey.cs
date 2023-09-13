using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddQuickslotKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuickslotKeys",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterQuickslotKeys, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\"}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuickslotKeys",
                table: "characters");
        }
    }
}
