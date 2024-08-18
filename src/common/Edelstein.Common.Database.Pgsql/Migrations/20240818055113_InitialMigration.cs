using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
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
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PIN = table.Column<string>(type: "text", nullable: true),
                    SPW = table.Column<string>(type: "text", nullable: true),
                    GradeCode = table.Column<byte>(type: "smallint", nullable: false),
                    SubGradeCode = table.Column<short>(type: "smallint", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: true),
                    NexonCash = table.Column<int>(type: "integer", nullable: false),
                    MaplePoint = table.Column<int>(type: "integer", nullable: false),
                    PrepaidNXCash = table.Column<int>(type: "integer", nullable: false)
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
