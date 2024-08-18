using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "identities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identities", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_identities_Username",
                table: "identities",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identities");
        }
    }
}
