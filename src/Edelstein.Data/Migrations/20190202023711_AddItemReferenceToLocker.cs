using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddItemReferenceToLocker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExpire",
                table: "ItemLockerSlot");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "ItemLockerSlot");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "ItemLockerSlot");

            migrationBuilder.AddColumn<int>(
                name: "ItemLockerSlotID",
                table: "ItemSlot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot",
                column: "ItemLockerSlotID",
                principalTable: "ItemLockerSlot",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLockerSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "ItemLockerSlotID",
                table: "ItemSlot");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpire",
                table: "ItemLockerSlot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemID",
                table: "ItemLockerSlot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "Number",
                table: "ItemLockerSlot",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
