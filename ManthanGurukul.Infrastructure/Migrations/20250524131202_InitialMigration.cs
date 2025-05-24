using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ManthanGurukul.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    MobileNo = table.Column<long>(type: "bigint", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "MobileNo", "ModifiedAt", "ModifiedBy", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("fecba826-3d32-44d9-960e-8fb7d3d5439d"), 638836891223980019L, new Guid("fecba826-3d32-44d9-960e-8fb7d3d5439d"), "nishant.kumar.mishra@gmail.com", "Nishant", true, "Mishra", 9945654327L, null, null, new byte[] { 141, 111, 248, 144, 4, 234, 12, 200, 235, 123, 142, 70, 188, 50, 30, 10, 20, 169, 12, 65, 236, 254, 95, 125, 222, 99, 208, 202, 214, 23, 117, 25, 62, 144, 126, 226, 69, 13, 61, 107, 61, 240, 181, 44, 68, 207, 83, 85, 38, 228, 6, 150, 148, 224, 23, 121, 61, 205, 231, 128, 62, 45, 250, 3 }, new byte[] { 176, 238, 135, 69, 73, 19, 140, 147, 146, 136, 41, 69, 227, 199, 133, 196, 98, 101, 45, 117, 127, 145, 219, 29, 194, 7, 255, 109, 151, 63, 104, 97, 76, 205, 95, 12, 27, 240, 96, 225, 217, 249, 106, 245, 58, 137, 136, 66, 24, 239, 218, 66, 222, 164, 88, 166, 184, 99, 228, 241, 179, 194, 14, 64, 135, 221, 70, 94, 16, 77, 236, 41, 41, 162, 40, 202, 50, 117, 91, 37, 115, 194, 7, 104, 69, 117, 76, 239, 245, 103, 254, 229, 93, 224, 211, 13, 7, 75, 179, 22, 241, 35, 170, 155, 187, 157, 52, 174, 124, 36, 147, 164, 196, 173, 177, 238, 131, 214, 250, 43, 233, 252, 22, 28, 80, 15, 206, 140 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_MobileNo_Email",
                table: "Users",
                columns: new[] { "MobileNo", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
