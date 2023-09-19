using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Social.Migrations
{
    /// <inheritdoc />
    public partial class AddFriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "friends",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterID = table.Column<int>(type: "integer", nullable: false),
                    FriendID = table.Column<int>(type: "integer", nullable: false),
                    FriendName = table.Column<string>(type: "text", nullable: false),
                    FriendGroup = table.Column<string>(type: "text", nullable: false),
                    Flag = table.Column<short>(type: "smallint", nullable: false),
                    ChannelID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friends", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_friends_CharacterID_FriendID",
                table: "friends",
                columns: new[] { "CharacterID", "FriendID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friends");
        }
    }
}
