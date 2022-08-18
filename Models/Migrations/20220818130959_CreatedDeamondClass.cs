using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class CreatedDeamondClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnClass_demand_DemandId",
                table: "salesReturnClass");

            migrationBuilder.RenameColumn(
                name: "DemandId",
                table: "salesReturnClass",
                newName: "DemandClassId");

            migrationBuilder.RenameIndex(
                name: "IX_salesReturnClass_DemandId",
                table: "salesReturnClass",
                newName: "IX_salesReturnClass_DemandClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_salesReturnClass_demand_DemandClassId",
                table: "salesReturnClass",
                column: "DemandClassId",
                principalTable: "demand",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnClass_demand_DemandClassId",
                table: "salesReturnClass");

            migrationBuilder.RenameColumn(
                name: "DemandClassId",
                table: "salesReturnClass",
                newName: "DemandId");

            migrationBuilder.RenameIndex(
                name: "IX_salesReturnClass_DemandClassId",
                table: "salesReturnClass",
                newName: "IX_salesReturnClass_DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_salesReturnClass_demand_DemandId",
                table: "salesReturnClass",
                column: "DemandId",
                principalTable: "demand",
                principalColumn: "Id");
        }
    }
}
