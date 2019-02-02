using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class MakeLockerOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.AlterColumn<int>(
                name: "ItemLockerSlotID",
                table: "ItemSlot",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                principalTable: "ItemLockerSlot",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.AlterColumn<int>(
                name: "ItemLockerSlotID",
                table: "ItemSlot",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                principalTable: "ItemLockerSlot",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
