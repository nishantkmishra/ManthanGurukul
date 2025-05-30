﻿// <auto-generated />
using System;
using ManthanGurukul.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ManthanGurukul.Infrastructure.Migrations
{
    [DbContext(typeof(ManthanGurukulDBContext))]
    [Migration("20250524131202_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ManthanGurukul.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ManthanGurukul.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<long>("MobileNo")
                        .HasColumnType("bigint");

                    b.Property<long?>("ModifiedAt")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("MobileNo", "Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fecba826-3d32-44d9-960e-8fb7d3d5439d"),
                            CreatedAt = 638836891223980019L,
                            CreatedBy = new Guid("fecba826-3d32-44d9-960e-8fb7d3d5439d"),
                            Email = "nishant.kumar.mishra@gmail.com",
                            FirstName = "Nishant",
                            IsActive = true,
                            LastName = "Mishra",
                            MobileNo = 9945654327L,
                            PasswordHash = new byte[] { 141, 111, 248, 144, 4, 234, 12, 200, 235, 123, 142, 70, 188, 50, 30, 10, 20, 169, 12, 65, 236, 254, 95, 125, 222, 99, 208, 202, 214, 23, 117, 25, 62, 144, 126, 226, 69, 13, 61, 107, 61, 240, 181, 44, 68, 207, 83, 85, 38, 228, 6, 150, 148, 224, 23, 121, 61, 205, 231, 128, 62, 45, 250, 3 },
                            PasswordSalt = new byte[] { 176, 238, 135, 69, 73, 19, 140, 147, 146, 136, 41, 69, 227, 199, 133, 196, 98, 101, 45, 117, 127, 145, 219, 29, 194, 7, 255, 109, 151, 63, 104, 97, 76, 205, 95, 12, 27, 240, 96, 225, 217, 249, 106, 245, 58, 137, 136, 66, 24, 239, 218, 66, 222, 164, 88, 166, 184, 99, 228, 241, 179, 194, 14, 64, 135, 221, 70, 94, 16, 77, 236, 41, 41, 162, 40, 202, 50, 117, 91, 37, 115, 194, 7, 104, 69, 117, 76, 239, 245, 103, 254, 229, 93, 224, 211, 13, 7, 75, 179, 22, 241, 35, 170, 155, 187, 157, 52, 174, 124, 36, 147, 164, 196, 173, 177, 238, 131, 214, 250, 43, 233, 252, 22, 28, 80, 15, 206, 140 }
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
