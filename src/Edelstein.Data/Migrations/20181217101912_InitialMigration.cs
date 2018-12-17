using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 13, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: true),
                    SecondPassword = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AccountData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountID = table.Column<int>(nullable: true),
                    WorldID = table.Column<byte>(nullable: false),
                    SlotCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountData_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 13, nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Skin = table.Column<byte>(nullable: false),
                    Face = table.Column<int>(nullable: false),
                    Hair = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Job = table.Column<short>(nullable: false),
                    STR = table.Column<short>(nullable: false),
                    DEX = table.Column<short>(nullable: false),
                    INT = table.Column<short>(nullable: false),
                    LUK = table.Column<short>(nullable: false),
                    HP = table.Column<int>(nullable: false),
                    MaxHP = table.Column<int>(nullable: false),
                    MP = table.Column<int>(nullable: false),
                    MaxMP = table.Column<int>(nullable: false),
                    AP = table.Column<short>(nullable: false),
                    SP = table.Column<short>(nullable: false),
                    EXP = table.Column<int>(nullable: false),
                    POP = table.Column<short>(nullable: false),
                    Money = table.Column<int>(nullable: false),
                    TempEXP = table.Column<int>(nullable: false),
                    FieldID = table.Column<int>(nullable: false),
                    FieldPortal = table.Column<byte>(nullable: false),
                    PlayTime = table.Column<int>(nullable: false),
                    SubJob = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_AccountData_DataID",
                        column: x => x.DataID,
                        principalTable: "AccountData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemInventory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    SlotMax = table.Column<byte>(nullable: false),
                    CharacterID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItemInventory_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSlot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemInventoryID = table.Column<int>(nullable: true),
                    Position = table.Column<short>(nullable: false),
                    TemplateID = table.Column<int>(nullable: false),
                    DateExpire = table.Column<DateTime>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Number = table.Column<short>(nullable: true),
                    MaxNumber = table.Column<short>(nullable: true),
                    Attribute = table.Column<short>(nullable: true),
                    Title = table.Column<string>(maxLength: 13, nullable: true),
                    RUC = table.Column<byte>(nullable: true),
                    CUC = table.Column<byte>(nullable: true),
                    STR = table.Column<short>(nullable: true),
                    DEX = table.Column<short>(nullable: true),
                    INT = table.Column<short>(nullable: true),
                    LUK = table.Column<short>(nullable: true),
                    MaxHP = table.Column<short>(nullable: true),
                    MaxMP = table.Column<short>(nullable: true),
                    PAD = table.Column<short>(nullable: true),
                    MAD = table.Column<short>(nullable: true),
                    PDD = table.Column<short>(nullable: true),
                    MDD = table.Column<short>(nullable: true),
                    ACC = table.Column<short>(nullable: true),
                    EVA = table.Column<short>(nullable: true),
                    Craft = table.Column<short>(nullable: true),
                    Speed = table.Column<short>(nullable: true),
                    Jump = table.Column<short>(nullable: true),
                    ItemSlotEquip_Title = table.Column<string>(nullable: true),
                    ItemSlotEquip_Attribute = table.Column<short>(nullable: true),
                    LevelUpType = table.Column<byte>(nullable: true),
                    Level = table.Column<byte>(nullable: true),
                    EXP = table.Column<int>(nullable: true),
                    Durability = table.Column<int>(nullable: true),
                    IUC = table.Column<int>(nullable: true),
                    Grade = table.Column<byte>(nullable: true),
                    CHUC = table.Column<byte>(nullable: true),
                    Option1 = table.Column<short>(nullable: true),
                    Option2 = table.Column<short>(nullable: true),
                    Option3 = table.Column<short>(nullable: true),
                    Socket1 = table.Column<short>(nullable: true),
                    Socket2 = table.Column<short>(nullable: true),
                    PetName = table.Column<string>(nullable: true),
                    ItemSlotPet_Level = table.Column<byte>(nullable: true),
                    Tameness = table.Column<short>(nullable: true),
                    Repleteness = table.Column<byte>(nullable: true),
                    DateDead = table.Column<DateTime>(nullable: true),
                    PetAttribute = table.Column<short>(nullable: true),
                    PetSkill = table.Column<short>(nullable: true),
                    RemainLife = table.Column<int>(nullable: true),
                    ItemSlotPet_Attribute = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSlot", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                        column: x => x.ItemInventoryID,
                        principalTable: "ItemInventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountData_AccountID",
                table: "AccountData",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_DataID",
                table: "Characters",
                column: "DataID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_CharacterID",
                table: "ItemInventory",
                column: "CharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemInventoryID",
                table: "ItemSlot",
                column: "ItemInventoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemInventory");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "AccountData");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
