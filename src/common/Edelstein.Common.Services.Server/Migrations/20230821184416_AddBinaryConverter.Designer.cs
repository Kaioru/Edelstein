﻿// <auto-generated />
using System;
using Edelstein.Common.Services.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edelstein.Common.Services.Server.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    [Migration("20230821184416_AddBinaryConverter")]
    partial class AddBinaryConverter
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.MigrationEntity", b =>
                {
                    b.Property<int>("CharacterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CharacterID"));

                    b.Property<byte[]>("Account")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("AccountID")
                        .HasColumnType("integer");

                    b.Property<byte[]>("AccountWorld")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Character")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("DateExpire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FromServerID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Key")
                        .HasColumnType("bigint");

                    b.Property<string>("ToServerID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CharacterID");

                    b.HasIndex("FromServerID");

                    b.HasIndex("ToServerID");

                    b.ToTable("migrations", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.ServerEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateExpire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("servers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Server");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.SessionEntity", b =>
                {
                    b.Property<int>("ActiveAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ActiveAccount"));

                    b.Property<int?>("ActiveCharacter")
                        .HasColumnType("integer");

                    b.Property<string>("ServerID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ActiveAccount");

                    b.HasIndex("ServerID");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.ServerGameEntity", b =>
                {
                    b.HasBaseType("Edelstein.Common.Services.Server.Entities.ServerEntity");

                    b.Property<int>("ChannelID")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAdultChannel")
                        .HasColumnType("boolean");

                    b.Property<int>("WorldID")
                        .HasColumnType("integer");

                    b.ToTable("servers", (string)null);

                    b.HasDiscriminator().HasValue("Game");
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.ServerLoginEntity", b =>
                {
                    b.HasBaseType("Edelstein.Common.Services.Server.Entities.ServerEntity");

                    b.ToTable("servers", (string)null);

                    b.HasDiscriminator().HasValue("Login");
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.MigrationEntity", b =>
                {
                    b.HasOne("Edelstein.Common.Services.Server.Entities.ServerEntity", "FromServer")
                        .WithMany("MigrationOut")
                        .HasForeignKey("FromServerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Edelstein.Common.Services.Server.Entities.ServerEntity", "ToServer")
                        .WithMany("MigrationIn")
                        .HasForeignKey("ToServerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromServer");

                    b.Navigation("ToServer");
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.SessionEntity", b =>
                {
                    b.HasOne("Edelstein.Common.Services.Server.Entities.ServerEntity", "Server")
                        .WithMany("Sessions")
                        .HasForeignKey("ServerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Edelstein.Common.Services.Server.Entities.ServerEntity", b =>
                {
                    b.Navigation("MigrationIn");

                    b.Navigation("MigrationOut");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
