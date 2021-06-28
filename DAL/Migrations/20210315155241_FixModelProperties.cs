using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixModelProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Unit_FactoryId",
                table: "Unit",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tank_UnitId",
                table: "Tank",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tank_Unit_UnitId",
                table: "Tank",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Factory_FactoryId",
                table: "Unit",
                column: "FactoryId",
                principalTable: "Factory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tank_Unit_UnitId",
                table: "Tank");

            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Factory_FactoryId",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Unit_FactoryId",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Tank_UnitId",
                table: "Tank");
        }
    }
}
