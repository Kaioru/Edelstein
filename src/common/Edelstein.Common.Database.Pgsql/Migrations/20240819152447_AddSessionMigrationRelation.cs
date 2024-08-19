using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionMigrationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "migration_info",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "integer", nullable: false),
                    AccountWorldDataID = table.Column<int>(type: "integer", nullable: false),
                    CharacterID = table.Column<int>(type: "integer", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FromServerID = table.Column<string>(type: "text", nullable: false),
                    ToServerID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_migration_info", x => new { x.AccountID, x.AccountWorldDataID, x.CharacterID });
                    table.ForeignKey(
                        name: "FK_migration_info_account_world_data_AccountWorldDataID",
                        column: x => x.AccountWorldDataID,
                        principalTable: "account_world_data",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_migration_info_accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_migration_info_server_info_FromServerID",
                        column: x => x.FromServerID,
                        principalTable: "server_info",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_migration_info_server_info_ToServerID",
                        column: x => x.ToServerID,
                        principalTable: "server_info",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_migration_info_session_info_AccountID",
                        column: x => x.AccountID,
                        principalTable: "session_info",
                        principalColumn: "ActiveAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_migration_info_AccountID",
                table: "migration_info",
                column: "AccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_migration_info_AccountWorldDataID",
                table: "migration_info",
                column: "AccountWorldDataID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_migration_info_CharacterID",
                table: "migration_info",
                column: "CharacterID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_migration_info_FromServerID",
                table: "migration_info",
                column: "FromServerID");

            migrationBuilder.CreateIndex(
                name: "IX_migration_info_ToServerID",
                table: "migration_info",
                column: "ToServerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "migration_info");
        }
    }
}
