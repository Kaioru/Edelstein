using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session_info",
                columns: table => new
                {
                    ActiveAccount = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerID = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveCharacter = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_info", x => x.ActiveAccount);
                    table.ForeignKey(
                        name: "FK_session_info_server_info_ServerID",
                        column: x => x.ServerID,
                        principalTable: "server_info",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_session_info_ServerID",
                table: "session_info",
                column: "ServerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session_info");
        }
    }
}
