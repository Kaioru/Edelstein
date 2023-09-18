using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wishlist",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWishlist, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0,0,0,0,0,0]}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wishlist",
                table: "characters");
        }
    }
}
