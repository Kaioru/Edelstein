using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporaryStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemporaryStats",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterTemporaryStats, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[Edelstein.Protocol.Gameplay.Models.Characters.Stats.TemporaryStatType, Edelstein.Protocol.Gameplay],[Edelstein.Protocol.Gameplay.Models.Characters.Stats.ITemporaryStatRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryStats",
                table: "characters");
        }
    }
}
