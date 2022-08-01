using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    public partial class AddMigrationGameData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AccountBytes",
                table: "Migrations",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "AccountWorldBytes",
                table: "Migrations",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "CharacterBytes",
                table: "Migrations",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBytes",
                table: "Migrations");

            migrationBuilder.DropColumn(
                name: "AccountWorldBytes",
                table: "Migrations");

            migrationBuilder.DropColumn(
                name: "CharacterBytes",
                table: "Migrations");
        }
    }
}
