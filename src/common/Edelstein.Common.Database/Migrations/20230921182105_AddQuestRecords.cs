using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestCompletes",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterQuestCompletes, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Characters.Quests.IQuestCompleteRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AddColumn<string>(
                name: "QuestRecords",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterQuestRecords, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Characters.Quests.IQuestRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AddColumn<string>(
                name: "QuestRecordsEx",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterQuestRecordsEx, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Characters.Quests.IQuestRecordEx, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestCompletes",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "QuestRecords",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "QuestRecordsEx",
                table: "characters");
        }
    }
}
