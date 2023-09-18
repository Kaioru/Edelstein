using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrunkAndLocker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"Money\":0,\"SlotMax\":4,\"Items\":{\"$type\":\"System.Collections.Generic.List`1[[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\",\"$values\":[]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"SlotMax\":4,\"Money\":0,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.List`1[[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\",\"$values\":[]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"SlotMax\":4,\"Money\":0,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"Money\":0,\"SlotMax\":4,\"Items\":{\"$type\":\"System.Collections.Generic.List`1[[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\",\"$values\":[]}}");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.List`1[[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\",\"$values\":[]}}");
        }
    }
}
