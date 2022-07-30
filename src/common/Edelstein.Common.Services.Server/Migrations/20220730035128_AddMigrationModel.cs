using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class AddMigrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Migrations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromServerID = table.Column<string>(type: "text", nullable: false),
                    ToServerID = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Migrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Migrations_Servers_FromServerID",
                        column: x => x.FromServerID,
                        principalTable: "Servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Migrations_Servers_ToServerID",
                        column: x => x.ToServerID,
                        principalTable: "Servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Migrations_FromServerID",
                table: "Migrations",
                column: "FromServerID");

            migrationBuilder.CreateIndex(
                name: "IX_Migrations_ToServerID",
                table: "Migrations",
                column: "ToServerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Migrations");
        }
    }
}
