using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddLockerToAccountData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CashItemSN",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemLockerID",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockerID",
                table: "AccountData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemLocker",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlotMax = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLocker", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemLockerID",
                table: "ItemSlot",
                column: "ItemLockerID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountData_LockerID",
                table: "AccountData",
                column: "LockerID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountData_ItemLocker_LockerID",
                table: "AccountData",
                column: "LockerID",
                principalTable: "ItemLocker",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_AccountData_ItemLocker_LockerID",
                table: "AccountData");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLocker_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemLocker");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_AccountData_LockerID",
                table: "AccountData");

            migrationBuilder.DropColumn(
                name: "CashItemSN",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "LockerID",
                table: "AccountData");
        }
    }
}
