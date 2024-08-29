using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterInventories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_account_world_data_AccountID",
                table: "account_world_data");

            migrationBuilder.AddColumn<string>(
                name: "Inventories",
                table: "characters",
                type: "jsonb",
                nullable: false,
                defaultValue: "{\"Equip\":{\"SlotMax\":24,\"Items\":{}},\"Consume\":{\"SlotMax\":24,\"Items\":{}},\"Install\":{\"SlotMax\":24,\"Items\":{}},\"Etc\":{\"SlotMax\":24,\"Items\":{}},\"Cash\":{\"SlotMax\":24,\"Items\":{}}}");

            migrationBuilder.CreateIndex(
                name: "IX_account_world_data_AccountID_WorldID",
                table: "account_world_data",
                columns: new[] { "AccountID", "WorldID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_account_world_data_AccountID_WorldID",
                table: "account_world_data");

            migrationBuilder.DropColumn(
                name: "Inventories",
                table: "characters");

            migrationBuilder.CreateIndex(
                name: "IX_account_world_data_AccountID",
                table: "account_world_data",
                column: "AccountID");
        }
    }
}
