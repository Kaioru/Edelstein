using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class RefactorLocker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemLockerSlot");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.RenameColumn(
                name: "ItemLockerSlotID",
                table: "ItemSlot",
                newName: "ItemLockerID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemLockerID",
                table: "ItemSlot",
                column: "ItemLockerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemLocker_ItemLockerID",
                table: "ItemSlot",
                column: "ItemLockerID",
                principalTable: "ItemLocker",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLocker_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.RenameColumn(
                name: "ItemLockerID",
                table: "ItemSlot",
                newName: "ItemLockerSlotID");

            migrationBuilder.CreateTable(
                name: "ItemLockerSlot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BuyCharacterName = table.Column<string>(nullable: true),
                    CommoditySN = table.Column<int>(nullable: false),
                    ItemLockerID = table.Column<int>(nullable: true),
                    SN = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLockerSlot", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItemLockerSlot_ItemLocker_ItemLockerID",
                        column: x => x.ItemLockerID,
                        principalTable: "ItemLocker",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemLockerSlot_ItemLockerID",
                table: "ItemLockerSlot",
                column: "ItemLockerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                principalTable: "ItemLockerSlot",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
