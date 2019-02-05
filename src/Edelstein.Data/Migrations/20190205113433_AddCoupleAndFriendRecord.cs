using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddCoupleAndFriendRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoupleRecord",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterID = table.Column<int>(nullable: false),
                    PairCharacterID = table.Column<int>(nullable: false),
                    PairCharacterName = table.Column<string>(nullable: true),
                    SN = table.Column<long>(nullable: false),
                    PairSN = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoupleRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FriendRecord",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterID = table.Column<int>(nullable: false),
                    PairCharacterID = table.Column<int>(nullable: false),
                    PairCharacterName = table.Column<string>(nullable: true),
                    SN = table.Column<long>(nullable: false),
                    PairSN = table.Column<long>(nullable: false),
                    FriendItemID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRecord", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoupleRecord");

            migrationBuilder.DropTable(
                name: "FriendRecord");
        }
    }
}
