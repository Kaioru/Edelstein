using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"Equip\":{\"SlotMax\":24,\"Items\":{}},\"Consume\":{\"SlotMax\":24,\"Items\":{}},\"Install\":{\"SlotMax\":24,\"Items\":{}},\"Etc\":{\"SlotMax\":24,\"Items\":{}},\"Cash\":{\"SlotMax\":24,\"Items\":{}}}",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldDefaultValue: "{\"Equip\":{\"SlotMax\":24,\"Items\":{}},\"Consume\":{\"SlotMax\":24,\"Items\":{}},\"Install\":{\"SlotMax\":24,\"Items\":{}},\"Etc\":{\"SlotMax\":24,\"Items\":{}},\"Cash\":{\"SlotMax\":24,\"Items\":{}}}");

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_world_data",
                type: "json",
                nullable: false,
                defaultValue: "{\"Money\":0,\"SlotMax\":4,\"Items\":[]}",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldDefaultValue: "{\"Money\":0,\"SlotMax\":4,\"Items\":[]}");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_world_data",
                type: "json",
                nullable: false,
                defaultValue: "{\"SlotMax\":999,\"Items\":[]}",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldDefaultValue: "{\"SlotMax\":999,\"Items\":[]}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "jsonb",
                nullable: false,
                defaultValue: "{\"Equip\":{\"SlotMax\":24,\"Items\":{}},\"Consume\":{\"SlotMax\":24,\"Items\":{}},\"Install\":{\"SlotMax\":24,\"Items\":{}},\"Etc\":{\"SlotMax\":24,\"Items\":{}},\"Cash\":{\"SlotMax\":24,\"Items\":{}}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"Equip\":{\"SlotMax\":24,\"Items\":{}},\"Consume\":{\"SlotMax\":24,\"Items\":{}},\"Install\":{\"SlotMax\":24,\"Items\":{}},\"Etc\":{\"SlotMax\":24,\"Items\":{}},\"Cash\":{\"SlotMax\":24,\"Items\":{}}}");

            migrationBuilder.AlterColumn<string>(
                name: "Trunk",
                table: "account_world_data",
                type: "jsonb",
                nullable: false,
                defaultValue: "{\"Money\":0,\"SlotMax\":4,\"Items\":[]}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"Money\":0,\"SlotMax\":4,\"Items\":[]}");

            migrationBuilder.AlterColumn<string>(
                name: "Locker",
                table: "account_world_data",
                type: "jsonb",
                nullable: false,
                defaultValue: "{\"SlotMax\":999,\"Items\":[]}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"SlotMax\":999,\"Items\":[]}");
        }
    }
}
