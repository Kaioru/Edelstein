using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWildHunterInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TemporaryStats",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterTemporaryStats, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[Edelstein.Protocol.Gameplay.Models.Characters.Stats.TemporaryStatType, Edelstein.Protocol.Gameplay],[Edelstein.Protocol.Gameplay.Models.Characters.Stats.ITemporaryStatRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"},\"EnergyChargedRecord\":null,\"DashSpeedRecord\":null,\"DashJumpRecord\":null,\"RideVehicleRecord\":null,\"PartyBoosterRecord\":null,\"GuidedBulletRecord\":null,\"UndeadRecord\":null}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterTemporaryStats, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[Edelstein.Protocol.Gameplay.Models.Characters.Stats.TemporaryStatType, Edelstein.Protocol.Gameplay],[Edelstein.Protocol.Gameplay.Models.Characters.Stats.ITemporaryStatRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AddColumn<string>(
                name: "WildHunterInfo",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWildHunterInfo, Edelstein.Common.Gameplay\",\"RidingType\":0,\"CaptureMob\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0]}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WildHunterInfo",
                table: "characters");

            migrationBuilder.AlterColumn<string>(
                name: "TemporaryStats",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterTemporaryStats, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[Edelstein.Protocol.Gameplay.Models.Characters.Stats.TemporaryStatType, Edelstein.Protocol.Gameplay],[Edelstein.Protocol.Gameplay.Models.Characters.Stats.ITemporaryStatRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterTemporaryStats, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[Edelstein.Protocol.Gameplay.Models.Characters.Stats.TemporaryStatType, Edelstein.Protocol.Gameplay],[Edelstein.Protocol.Gameplay.Models.Characters.Stats.ITemporaryStatRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"},\"EnergyChargedRecord\":null,\"DashSpeedRecord\":null,\"DashJumpRecord\":null,\"RideVehicleRecord\":null,\"PartyBoosterRecord\":null,\"GuidedBulletRecord\":null,\"UndeadRecord\":null}");
        }
    }
}
