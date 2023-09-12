using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDefaultValuesOfJSONColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterSkills, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Characters.Skills.ISkillRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterInventories, Edelstein.Common.Gameplay\",\"Equip\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Consume\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Install\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Etc\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Cash\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtendSP",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"SlotMax\":4,\"Money\":0,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "characters",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterSkills, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Characters.Skills.ISkillRecord, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterInventories, Edelstein.Common.Gameplay\",\"Equip\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Consume\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Install\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Etc\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}},\"Cash\":{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemInventory, Edelstein.Common.Gameplay\",\"SlotMax\":24,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}}");

            migrationBuilder.AlterColumn<string>(
                name: "ExtendSP",
                table: "characters",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterExtendSP, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\"}}");

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_worlds",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemTrunk, Edelstein.Common.Gameplay\",\"SlotMax\":4,\"Money\":0,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.Items.IItemSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_worlds",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Inventories.ItemLocker, Edelstein.Common.Gameplay\",\"SlotMax\":999,\"Items\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int16, System.Private.CoreLib],[Edelstein.Protocol.Gameplay.Models.Inventories.IItemLockerSlot, Edelstein.Protocol.Gameplay]], System.Private.CoreLib\"}}");
        }
    }
}
