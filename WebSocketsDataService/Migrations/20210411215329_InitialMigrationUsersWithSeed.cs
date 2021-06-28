using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSocketsDataService.Migrations
{
    public partial class InitialMigrationUsersWithSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserClaims_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "Administrator" },
                    { "2", "Manager" },
                    { "3", "User" },
                    { "4", "Service" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "HashedPassword", "Login", "Name", "RoleId" },
                values: new object[] { "1", "AQAAAAEAACcQAAAAEPO20MgnC3bgnulxetBENGuxg8hjRXXdJRoB9dEXDoSBM2NA3ek6vImtDKouDdEOAA==", "admin", "admin", "1" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserClaims");
        }
    }
}
