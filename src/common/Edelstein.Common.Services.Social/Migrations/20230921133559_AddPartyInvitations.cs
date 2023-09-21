using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Social.Migrations
{
    /// <inheritdoc />
    public partial class AddPartyInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "party_invitations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyID = table.Column<int>(type: "integer", nullable: false),
                    CharacterID = table.Column<int>(type: "integer", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party_invitations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_party_invitations_parties_PartyID",
                        column: x => x.PartyID,
                        principalTable: "parties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_party_invitations_PartyID_CharacterID",
                table: "party_invitations",
                columns: new[] { "PartyID", "CharacterID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "party_invitations");
        }
    }
}
