using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class ReformatItemLocker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemLocker_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "ItemLockerID",
                table: "ItemSlot");

            migrationBuilder.CreateTable(
                name: "ItemLockerSlot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemID = table.Column<int>(nullable: false),
                    CommoditySN = table.Column<int>(nullable: false),
                    BuyCharacterName = table.Column<string>(nullable: true),
                    DateExpire = table.Column<DateTime>(nullable: true),
                    ItemLockerID = table.Column<int>(nullable: true)
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
                name: "IX_ItemLockerSlot_ItemLockerID",
                table: "ItemLockerSlot",
                column: "ItemLockerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLockerSlot");

            migrationBuilder.AddColumn<int>(
                name: "ItemLockerID",
                table: "ItemSlot",
                nullable: true);

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
    }
}
