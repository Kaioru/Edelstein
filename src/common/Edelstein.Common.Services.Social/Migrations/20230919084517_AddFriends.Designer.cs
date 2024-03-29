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
    [Migration("20230919084517_AddFriends")]
    partial class AddFriends
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
#pragma warning restore 612, 618
        }
    }
}
