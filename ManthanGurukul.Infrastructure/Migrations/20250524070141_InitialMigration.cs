using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "User",
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
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "MobileNo", "ModifiedAt", "ModifiedBy", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("16e0b0e5-792f-4f11-beec-cfe9340fe4be"), 638836669011345171L, new Guid("16e0b0e5-792f-4f11-beec-cfe9340fe4be"), "nishant.kumar.mishra@gmail.com", "Nishant", true, "Mishra", 9945654327L, 0L, new Guid("00000000-0000-0000-0000-000000000000"), new byte[] { 125, 224, 34, 193, 214, 148, 94, 129, 17, 191, 150, 112, 110, 54, 108, 193, 109, 62, 25, 152, 219, 44, 66, 215, 235, 254, 46, 99, 26, 105, 60, 154, 15, 212, 235, 74, 170, 208, 187, 194, 50, 239, 77, 228, 96, 101, 67, 137, 43, 84, 246, 179, 93, 83, 45, 0, 8, 50, 169, 201, 208, 68, 33, 123 }, new byte[] { 142, 25, 152, 55, 28, 166, 3, 170, 139, 2, 13, 0, 187, 0, 221, 188, 10, 13, 227, 143, 155, 225, 12, 85, 226, 26, 146, 155, 241, 9, 149, 137, 211, 249, 236, 158, 120, 52, 152, 219, 18, 123, 100, 89, 76, 7, 38, 37, 31, 151, 143, 135, 93, 2, 248, 40, 198, 51, 34, 99, 100, 98, 81, 103, 206, 103, 239, 106, 202, 86, 11, 42, 155, 185, 165, 108, 227, 114, 113, 109, 76, 142, 56, 247, 154, 5, 248, 6, 108, 220, 68, 17, 18, 215, 116, 148, 195, 118, 113, 163, 66, 211, 225, 186, 118, 193, 61, 27, 143, 183, 171, 64, 118, 208, 186, 186, 195, 73, 149, 179, 116, 63, 129, 106, 255, 22, 21, 172 } });

            migrationBuilder.CreateIndex(
                name: "IX_User_MobileNo_Email",
                table: "User",
                columns: new[] { "MobileNo", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
