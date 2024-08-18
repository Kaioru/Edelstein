using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PIN = table.Column<string>(type: "TEXT", nullable: true),
                    SPW = table.Column<string>(type: "TEXT", nullable: true),
                    GradeCode = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubGradeCode = table.Column<short>(type: "INTEGER", nullable: false),
                    Gender = table.Column<byte>(type: "INTEGER", nullable: true),
                    NexonCash = table.Column<int>(type: "INTEGER", nullable: false),
                    MaplePoint = table.Column<int>(type: "INTEGER", nullable: false),
                    PrepaidNXCash = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Username",
                table: "accounts",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
