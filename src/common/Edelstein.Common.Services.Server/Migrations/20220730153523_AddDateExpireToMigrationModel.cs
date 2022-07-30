using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class AddDateExpireToMigrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpire",
                table: "Migrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Migrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExpire",
                table: "Migrations");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Migrations");
        }
    }
}
