using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Session.Migrations
{
    public partial class AddTimestampsToSessionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpire",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExpire",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Sessions");
        }
    }
}
