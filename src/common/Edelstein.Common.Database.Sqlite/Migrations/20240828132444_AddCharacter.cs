using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActiveAccount",
                table: "session_info",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountWorldDataID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<byte>(type: "INTEGER", nullable: false),
                    Skin = table.Column<byte>(type: "INTEGER", nullable: false),
                    Face = table.Column<int>(type: "INTEGER", nullable: false),
                    Hair = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<byte>(type: "INTEGER", nullable: false),
                    Job = table.Column<short>(type: "INTEGER", nullable: false),
                    STR = table.Column<short>(type: "INTEGER", nullable: false),
                    DEX = table.Column<short>(type: "INTEGER", nullable: false),
                    INT = table.Column<short>(type: "INTEGER", nullable: false),
                    LUK = table.Column<short>(type: "INTEGER", nullable: false),
                    HP = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHP = table.Column<int>(type: "INTEGER", nullable: false),
                    MP = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxMP = table.Column<int>(type: "INTEGER", nullable: false),
                    AP = table.Column<short>(type: "INTEGER", nullable: false),
                    SP = table.Column<short>(type: "INTEGER", nullable: false),
                    EXP = table.Column<int>(type: "INTEGER", nullable: false),
                    POP = table.Column<short>(type: "INTEGER", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    TempEXP = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldID = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldPortal = table.Column<byte>(type: "INTEGER", nullable: false),
                    PlayTime = table.Column<int>(type: "INTEGER", nullable: false),
                    SubJob = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_characters_account_world_data_AccountWorldDataID",
                        column: x => x.AccountWorldDataID,
                        principalTable: "account_world_data",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_session_info_ActiveCharacter",
                table: "session_info",
                column: "ActiveCharacter",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_characters_AccountWorldDataID",
                table: "characters",
                column: "AccountWorldDataID");

            migrationBuilder.CreateIndex(
                name: "IX_characters_Name",
                table: "characters",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_migration_info_characters_CharacterID",
                table: "migration_info",
                column: "CharacterID",
                principalTable: "characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_session_info_accounts_ActiveAccount",
                table: "session_info",
                column: "ActiveAccount",
                principalTable: "accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_session_info_characters_ActiveCharacter",
                table: "session_info",
                column: "ActiveCharacter",
                principalTable: "characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_migration_info_characters_CharacterID",
                table: "migration_info");

            migrationBuilder.DropForeignKey(
                name: "FK_session_info_accounts_ActiveAccount",
                table: "session_info");

            migrationBuilder.DropForeignKey(
                name: "FK_session_info_characters_ActiveCharacter",
                table: "session_info");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropIndex(
                name: "IX_session_info_ActiveCharacter",
                table: "session_info");

            migrationBuilder.AlterColumn<int>(
                name: "ActiveAccount",
                table: "session_info",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
