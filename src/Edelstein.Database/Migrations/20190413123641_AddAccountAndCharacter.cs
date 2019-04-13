using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddAccountAndCharacter : Migration
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
                    SecondPassword = table.Column<string>(maxLength: 128, nullable: true),
                    Gender = table.Column<byte>(nullable: true),
                    NexonCash = table.Column<int>(nullable: false),
                    MaplePoint = table.Column<int>(nullable: false),
                    PrepaidNXCash = table.Column<int>(nullable: false),
                    LatestConnectedWorld = table.Column<byte>(nullable: false),
                    LatestConnectedService = table.Column<string>(nullable: true),
                    PreviousConnectedService = table.Column<string>(nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Character",
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
                    table.PrimaryKey("PK_Character", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Character_AccountData_DataID",
                        column: x => x.DataID,
                        principalTable: "AccountData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountData_AccountID",
                table: "AccountData",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Character_DataID",
                table: "Character",
                column: "DataID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "AccountData");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
