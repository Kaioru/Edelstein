using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
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
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountWorldDataID = table.Column<int>(type: "integer", nullable: false),
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
                    SubJob = table.Column<short>(type: "smallint", nullable: false)
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
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
