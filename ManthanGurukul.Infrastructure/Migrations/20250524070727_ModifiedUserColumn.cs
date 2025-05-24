using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManthanGurukul.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("16e0b0e5-792f-4f11-beec-cfe9340fe4be"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "User",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedAt",
                table: "User",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "MobileNo", "ModifiedAt", "ModifiedBy", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("a4715f47-7de9-45e1-80fc-c6ea6e6eadde"), 638836672468805628L, new Guid("a4715f47-7de9-45e1-80fc-c6ea6e6eadde"), "nishant.kumar.mishra@gmail.com", "Nishant", true, "Mishra", 9945654327L, null, null, new byte[] { 166, 47, 200, 85, 51, 238, 238, 80, 81, 102, 36, 176, 34, 225, 121, 132, 118, 242, 164, 179, 175, 17, 50, 88, 154, 0, 114, 142, 171, 143, 253, 173, 220, 54, 250, 189, 103, 62, 212, 146, 30, 38, 59, 45, 0, 250, 200, 196, 98, 211, 110, 133, 167, 167, 189, 157, 7, 11, 56, 73, 44, 94, 215, 220 }, new byte[] { 105, 90, 183, 69, 204, 241, 196, 168, 36, 49, 221, 118, 238, 204, 97, 92, 164, 169, 248, 130, 56, 55, 221, 183, 175, 88, 128, 127, 90, 13, 34, 222, 232, 184, 230, 177, 215, 89, 54, 29, 146, 50, 168, 225, 42, 150, 184, 147, 66, 169, 184, 227, 165, 226, 22, 178, 134, 134, 223, 123, 79, 111, 110, 105, 144, 104, 127, 168, 35, 170, 143, 235, 184, 224, 165, 252, 190, 117, 247, 109, 142, 54, 164, 140, 187, 9, 138, 129, 39, 119, 121, 61, 60, 72, 215, 75, 175, 211, 17, 198, 174, 182, 119, 78, 38, 199, 14, 216, 177, 189, 223, 230, 141, 224, 77, 126, 180, 108, 229, 198, 200, 135, 139, 31, 77, 170, 164, 1 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("a4715f47-7de9-45e1-80fc-c6ea6e6eadde"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedAt",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "MobileNo", "ModifiedAt", "ModifiedBy", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("16e0b0e5-792f-4f11-beec-cfe9340fe4be"), 638836669011345171L, new Guid("16e0b0e5-792f-4f11-beec-cfe9340fe4be"), "nishant.kumar.mishra@gmail.com", "Nishant", true, "Mishra", 9945654327L, 0L, new Guid("00000000-0000-0000-0000-000000000000"), new byte[] { 125, 224, 34, 193, 214, 148, 94, 129, 17, 191, 150, 112, 110, 54, 108, 193, 109, 62, 25, 152, 219, 44, 66, 215, 235, 254, 46, 99, 26, 105, 60, 154, 15, 212, 235, 74, 170, 208, 187, 194, 50, 239, 77, 228, 96, 101, 67, 137, 43, 84, 246, 179, 93, 83, 45, 0, 8, 50, 169, 201, 208, 68, 33, 123 }, new byte[] { 142, 25, 152, 55, 28, 166, 3, 170, 139, 2, 13, 0, 187, 0, 221, 188, 10, 13, 227, 143, 155, 225, 12, 85, 226, 26, 146, 155, 241, 9, 149, 137, 211, 249, 236, 158, 120, 52, 152, 219, 18, 123, 100, 89, 76, 7, 38, 37, 31, 151, 143, 135, 93, 2, 248, 40, 198, 51, 34, 99, 100, 98, 81, 103, 206, 103, 239, 106, 202, 86, 11, 42, 155, 185, 165, 108, 227, 114, 113, 109, 76, 142, 56, 247, 154, 5, 248, 6, 108, 220, 68, 17, 18, 215, 116, 148, 195, 118, 113, 163, 66, 211, 225, 186, 118, 193, 61, 27, 143, 183, 171, 64, 118, 208, 186, 186, 195, 73, 149, 179, 116, 63, 129, 106, 255, 22, 21, 172 } });
        }
    }
}
