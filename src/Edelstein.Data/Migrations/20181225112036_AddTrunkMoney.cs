using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Data.Migrations
{
    public partial class AddTrunkMoney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Money",
                table: "ItemTrunk",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "ItemTrunk");
        }
    }
}
