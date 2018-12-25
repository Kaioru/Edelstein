using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddItemTrunk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemTrunkID",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrunkID",
                table: "AccountData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemTrunk",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlotMax = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTrunk", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemTrunkID",
                table: "ItemSlot",
                column: "ItemTrunkID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountData_TrunkID",
                table: "AccountData",
                column: "TrunkID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountData_ItemTrunk_TrunkID",
                table: "AccountData",
                column: "TrunkID",
                principalTable: "ItemTrunk",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemTrunk_ItemTrunkID",
                table: "ItemSlot",
                column: "ItemTrunkID",
                principalTable: "ItemTrunk",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountData_ItemTrunk_TrunkID",
                table: "AccountData");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemTrunk_ItemTrunkID",
                table: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemTrunk");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemTrunkID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_AccountData_TrunkID",
                table: "AccountData");

            migrationBuilder.DropColumn(
                name: "ItemTrunkID",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "TrunkID",
                table: "AccountData");
        }
    }
}
