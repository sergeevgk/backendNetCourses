using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1498ccde-9b5c-4894-8dfb-68fadafec12e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "2f895aa4-6fe8-41a4-a293-b07dad6af010", "Administrator", "ADMINISTRATOR" },
                    { "2", "c15e4139-154d-4c5e-b474-492c2aa4f11a", "Manager", "MANAGER" },
                    { "3", "2a3310de-027b-416e-9bcf-2b6b2c029be1", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "c0c191aa-3e0e-4835-a04f-54c1e80b16b3", "admin123@mail.ru", false, false, null, null, null, "AQAAAAEAACcQAAAAEPlZry0H9y2lC79iu7ynVPK5mr2vnueGa8K7DqqQotFrWOSYjaW3iWlRwPdfeY5Jog==", null, false, "ac191553-7fa1-441d-9389-7597ac2aa4e7", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1498ccde-9b5c-4894-8dfb-68fadafec12e", 0, "7b00b56d-3092-43c2-bc95-9753383c46ee", "admin123@mail.ru", false, false, null, null, null, "AQAAAAEAACcQAAAAENhIIIxZre5d1EjQ2At7U0DFzm1olvNyya5p9VIF5s9tYxoaozDuWJNfb24PCgB1Zg==", null, false, "7c6b388e-2e82-451c-8846-9f259735b6b4", false, "admin" });
        }
    }
}
