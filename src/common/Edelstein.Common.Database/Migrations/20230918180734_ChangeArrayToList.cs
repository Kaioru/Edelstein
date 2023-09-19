using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeArrayToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Wishlist",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWishlist, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWishlist, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0,0,0,0,0,0]}}");

            migrationBuilder.AlterColumn<string>(
                name: "WildHunterInfo",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWildHunterInfo, Edelstein.Common.Gameplay\",\"RidingType\":0,\"CaptureMob\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWildHunterInfo, Edelstein.Common.Gameplay\",\"RidingType\":0,\"CaptureMob\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0]}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Wishlist",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWishlist, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0,0,0,0,0,0]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWishlist, Edelstein.Common.Gameplay\",\"Records\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[]}}");

            migrationBuilder.AlterColumn<string>(
                name: "WildHunterInfo",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWildHunterInfo, Edelstein.Common.Gameplay\",\"RidingType\":0,\"CaptureMob\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[0,0,0,0,0]}}",
                oldClrType: typeof(string),
                oldType: "json",
                oldDefaultValue: "{\"$type\":\"Edelstein.Common.Gameplay.Models.Characters.CharacterWildHunterInfo, Edelstein.Common.Gameplay\",\"RidingType\":0,\"CaptureMob\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[]}}");
        }
    }
}
