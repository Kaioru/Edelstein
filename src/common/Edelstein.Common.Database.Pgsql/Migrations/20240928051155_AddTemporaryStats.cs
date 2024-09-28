using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporaryStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemporaryStats",
                table: "characters",
                type: "json",
                nullable: false,
                defaultValue: "{\"Records\":{},\"DiceInfo\":null,\"EnergyCharged\":null,\"DashSpeed\":null,\"DashJump\":null,\"RideVehicle\":null,\"PartyBooster\":null,\"GuidedBullet\":null,\"Undead\":null}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryStats",
                table: "characters");
        }
    }
}
