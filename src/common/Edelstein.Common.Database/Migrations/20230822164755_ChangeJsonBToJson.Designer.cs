﻿// <auto-generated />
using System;
using Edelstein.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Database.Migrations
{
    [DbContext(typeof(GameplayDbContext))]
    [Migration("20230822164755_ChangeJsonBToJson")]
    partial class ChangeJsonBToJson
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Edelstein.Common.Database.Entities.AccountEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<byte?>("Gender")
                        .HasColumnType("smallint");

                    b.Property<byte>("GradeCode")
                        .HasColumnType("smallint");

                    b.Property<string>("PIN")
                        .HasColumnType("text");

                    b.Property<string>("SPW")
                        .HasColumnType("text");

                    b.Property<short>("SubGradeCode")
                        .HasColumnType("smallint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.AccountWorldEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("integer");

                    b.Property<int>("CharacterSlotMax")
                        .HasColumnType("integer");

                    b.Property<string>("Locker")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("Trunk")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<int>("WorldID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("account_worlds", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.CharacterEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<short>("AP")
                        .HasColumnType("smallint");

                    b.Property<int>("AccountWorldID")
                        .HasColumnType("integer");

                    b.Property<short>("DEX")
                        .HasColumnType("smallint");

                    b.Property<int>("EXP")
                        .HasColumnType("integer");

                    b.Property<int>("Face")
                        .HasColumnType("integer");

                    b.Property<int>("FieldID")
                        .HasColumnType("integer");

                    b.Property<byte>("FieldPortal")
                        .HasColumnType("smallint");

                    b.Property<byte>("Gender")
                        .HasColumnType("smallint");

                    b.Property<int>("HP")
                        .HasColumnType("integer");

                    b.Property<int>("Hair")
                        .HasColumnType("integer");

                    b.Property<short>("INT")
                        .HasColumnType("smallint");

                    b.Property<string>("Inventories")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<short>("Job")
                        .HasColumnType("smallint");

                    b.Property<short>("LUK")
                        .HasColumnType("smallint");

                    b.Property<byte>("Level")
                        .HasColumnType("smallint");

                    b.Property<int>("MP")
                        .HasColumnType("integer");

                    b.Property<int>("MaxHP")
                        .HasColumnType("integer");

                    b.Property<int>("MaxMP")
                        .HasColumnType("integer");

                    b.Property<int>("Money")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short>("POP")
                        .HasColumnType("smallint");

                    b.Property<int>("PlayTime")
                        .HasColumnType("integer");

                    b.Property<short>("SP")
                        .HasColumnType("smallint");

                    b.Property<short>("STR")
                        .HasColumnType("smallint");

                    b.Property<byte>("Skin")
                        .HasColumnType("smallint");

                    b.Property<short>("SubJob")
                        .HasColumnType("smallint");

                    b.Property<int>("TempEXP")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("AccountWorldID");

                    b.ToTable("characters", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.AccountWorldEntity", b =>
                {
                    b.HasOne("Edelstein.Common.Database.Entities.AccountEntity", "Account")
                        .WithMany("AccountWorlds")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.CharacterEntity", b =>
                {
                    b.HasOne("Edelstein.Common.Database.Entities.AccountWorldEntity", "AccountWorld")
                        .WithMany("Characters")
                        .HasForeignKey("AccountWorldID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountWorld");
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.AccountEntity", b =>
                {
                    b.Navigation("AccountWorlds");
                });

            modelBuilder.Entity("Edelstein.Common.Database.Entities.AccountWorldEntity", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
