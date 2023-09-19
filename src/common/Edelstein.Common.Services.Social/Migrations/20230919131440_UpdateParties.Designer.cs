﻿// <auto-generated />
using Edelstein.Common.Services.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Social.Migrations
{
    [DbContext(typeof(SocialDbContext))]
    [Migration("20230919131440_UpdateParties")]
    partial class UpdateParties
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Edelstein.Common.Services.Social.Entities.FriendEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("ChannelID")
                        .HasColumnType("integer");

                    b.Property<int>("CharacterID")
                        .HasColumnType("integer");

                    b.Property<short>("Flag")
                        .HasColumnType("smallint");

                    b.Property<string>("FriendGroup")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FriendID")
                        .HasColumnType("integer");

                    b.Property<string>("FriendName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("CharacterID", "FriendID")
                        .IsUnique();

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Services.Social.Entities.PartyEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("BossCharacterID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("parties", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Services.Social.Entities.PartyMemberEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("ChannelID")
                        .HasColumnType("integer");

                    b.Property<int>("CharacterID")
                        .HasColumnType("integer");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Job")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("PartyID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("CharacterID")
                        .IsUnique();

                    b.HasIndex("PartyID");

                    b.ToTable("party_members", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Services.Social.Entities.PartyMemberEntity", b =>
                {
                    b.HasOne("Edelstein.Common.Services.Social.Entities.PartyEntity", "Party")
                        .WithMany("Members")
                        .HasForeignKey("PartyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("Edelstein.Common.Services.Social.Entities.PartyEntity", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
