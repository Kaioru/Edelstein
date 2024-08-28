using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Database.Pgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddServerInfoGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelID",
                table: "server_info",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdultChannel",
                table: "server_info",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldID",
                table: "server_info",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "server_info");

            migrationBuilder.DropColumn(
                name: "IsAdultChannel",
                table: "server_info");

            migrationBuilder.DropColumn(
                name: "WorldID",
                table: "server_info");
        }
    }
}
