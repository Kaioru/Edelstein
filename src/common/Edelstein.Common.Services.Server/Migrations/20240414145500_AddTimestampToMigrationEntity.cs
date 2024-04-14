using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampToMigrationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpire",
                table: "migrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "migrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExpire",
                table: "migrations");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "migrations");
        }
    }
}
