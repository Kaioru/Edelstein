using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Inventories;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PIN = table.Column<string>(type: "text", nullable: true),
                    SPW = table.Column<string>(type: "text", nullable: true),
                    GradeCode = table.Column<byte>(type: "smallint", nullable: false),
                    SubGradeCode = table.Column<short>(type: "smallint", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "account_worlds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountID = table.Column<int>(type: "integer", nullable: false),
                    WorldID = table.Column<int>(type: "integer", nullable: false),
                    Locker = table.Column<IItemLocker>(type: "jsonb", nullable: false),
                    Trunk = table.Column<IItemTrunk>(type: "jsonb", nullable: false),
                    CharacterSlotMax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_worlds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_account_worlds_accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountWorldID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: false),
                    Skin = table.Column<byte>(type: "smallint", nullable: false),
                    Face = table.Column<int>(type: "integer", nullable: false),
                    Hair = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<byte>(type: "smallint", nullable: false),
                    Job = table.Column<short>(type: "smallint", nullable: false),
                    STR = table.Column<short>(type: "smallint", nullable: false),
                    DEX = table.Column<short>(type: "smallint", nullable: false),
                    INT = table.Column<short>(type: "smallint", nullable: false),
                    LUK = table.Column<short>(type: "smallint", nullable: false),
                    HP = table.Column<int>(type: "integer", nullable: false),
                    MaxHP = table.Column<int>(type: "integer", nullable: false),
                    MP = table.Column<int>(type: "integer", nullable: false),
                    MaxMP = table.Column<int>(type: "integer", nullable: false),
                    AP = table.Column<short>(type: "smallint", nullable: false),
                    SP = table.Column<short>(type: "smallint", nullable: false),
                    EXP = table.Column<int>(type: "integer", nullable: false),
                    POP = table.Column<short>(type: "smallint", nullable: false),
                    Money = table.Column<int>(type: "integer", nullable: false),
                    TempEXP = table.Column<int>(type: "integer", nullable: false),
                    FieldID = table.Column<int>(type: "integer", nullable: false),
                    FieldPortal = table.Column<byte>(type: "smallint", nullable: false),
                    PlayTime = table.Column<int>(type: "integer", nullable: false),
                    SubJob = table.Column<short>(type: "smallint", nullable: false),
                    Inventories = table.Column<IDictionary<ItemInventoryType, IItemInventory>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_characters_account_worlds_AccountWorldID",
                        column: x => x.AccountWorldID,
                        principalTable: "account_worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_worlds_AccountID",
                table: "account_worlds",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Username",
                table: "accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_characters_AccountWorldID",
                table: "characters",
                column: "AccountWorldID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "account_worlds");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
