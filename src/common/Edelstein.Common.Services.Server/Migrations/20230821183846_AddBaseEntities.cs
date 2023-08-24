using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    WorldID = table.Column<int>(type: "integer", nullable: true),
                    ChannelID = table.Column<int>(type: "integer", nullable: true),
                    IsAdultChannel = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "migrations",
                columns: table => new
                {
                    CharacterID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountID = table.Column<int>(type: "integer", nullable: false),
                    FromServerID = table.Column<string>(type: "text", nullable: false),
                    ToServerID = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<long>(type: "bigint", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_migrations", x => x.CharacterID);
                    table.ForeignKey(
                        name: "FK_migrations_servers_FromServerID",
                        column: x => x.FromServerID,
                        principalTable: "servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_migrations_servers_ToServerID",
                        column: x => x.ToServerID,
                        principalTable: "servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    ActiveAccount = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActiveCharacter = table.Column<int>(type: "integer", nullable: true),
                    ServerID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.ActiveAccount);
                    table.ForeignKey(
                        name: "FK_sessions_servers_ServerID",
                        column: x => x.ServerID,
                        principalTable: "servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_migrations_FromServerID",
                table: "migrations",
                column: "FromServerID");

            migrationBuilder.CreateIndex(
                name: "IX_migrations_ToServerID",
                table: "migrations",
                column: "ToServerID");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_ServerID",
                table: "sessions",
                column: "ServerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "migrations");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "servers");
        }
    }
}
