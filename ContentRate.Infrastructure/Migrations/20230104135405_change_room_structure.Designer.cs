﻿// <auto-generated />
using System;
using ContentRate.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContentRate.Infrastructure.Migrations
{
    [DbContext(typeof(ContentRateDbContext))]
    [Migration("20230104135405_change_room_structure")]
    partial class changeroomstructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ContentRate.Infrastructure.Models.ContentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Content", (string)null);
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.NotificationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<bool>("IsWatched")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.RoomModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms", (string)null);
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsMockUser")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RoomModelUserModel", b =>
                {
                    b.Property<Guid>("RoomsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("RoomsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoomModelUserModel");
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.ContentModel", b =>
                {
                    b.HasOne("ContentRate.Infrastructure.Models.RoomModel", "Room")
                        .WithMany("ContentList")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("ContentRate.Infrastructure.Models.RatingModel", "Ratings", b1 =>
                        {
                            b1.Property<Guid>("ContentModelId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Value")
                                .HasColumnType("double precision");

                            b1.HasKey("ContentModelId", "Id");

                            b1.ToTable("Ratings", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ContentModelId");
                        });

                    b.Navigation("Ratings");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.NotificationModel", b =>
                {
                    b.HasOne("ContentRate.Infrastructure.Models.UserModel", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.RoomModel", b =>
                {
                    b.OwnsOne("ContentRate.Infrastructure.Models.RoomDetailsModel", "RoomDetails", b1 =>
                        {
                            b1.Property<Guid>("RoomModelId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreationTime")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<Guid>("CreatorId")
                                .HasColumnType("uuid");

                            b1.Property<bool>("IsPrivate")
                                .HasColumnType("boolean");

                            b1.Property<string>("Password")
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)");

                            b1.HasKey("RoomModelId");

                            b1.ToTable("RoomDetails", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomModelId");
                        });

                    b.Navigation("RoomDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("RoomModelUserModel", b =>
                {
                    b.HasOne("ContentRate.Infrastructure.Models.RoomModel", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentRate.Infrastructure.Models.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.RoomModel", b =>
                {
                    b.Navigation("ContentList");
                });

            modelBuilder.Entity("ContentRate.Infrastructure.Models.UserModel", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
