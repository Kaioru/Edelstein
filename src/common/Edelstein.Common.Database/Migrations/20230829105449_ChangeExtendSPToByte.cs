using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExtendSPToByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExtendSP",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Byte, System.Private.CoreLib],[System.Byte, System.Private.CoreLib]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\"}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExtendSP",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Byte, System.Private.CoreLib],[System.Byte, System.Private.CoreLib]], System.Private.CoreLib\"}}");
        }
    }
}
