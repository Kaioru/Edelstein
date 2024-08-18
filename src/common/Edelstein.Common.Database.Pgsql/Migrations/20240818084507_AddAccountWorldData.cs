using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountWorldData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_world_data",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountID = table.Column<int>(type: "integer", nullable: false),
                    WorldID = table.Column<int>(type: "integer", nullable: false),
                    Locker = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{\"SlotMax\":999,\"Items\":[]}"),
                    Trunk = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{\"Money\":0,\"SlotMax\":4,\"Items\":[]}"),
                    CharacterSlotMax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_world_data", x => x.ID);
                    table.ForeignKey(
                        name: "FK_account_world_data_accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_world_data_AccountID",
                table: "account_world_data",
                column: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_world_data");
        }
    }
}
