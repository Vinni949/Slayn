using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class CreationOrderClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderClass_demand_DemandId",
                table: "orderClass");

            migrationBuilder.DropIndex(
                name: "IX_orderClass_DemandId",
                table: "orderClass");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "orderClass");

            migrationBuilder.AddColumn<string>(
                name: "OrderClassId",
                table: "demand",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_demand_OrderClassId",
                table: "demand",
                column: "OrderClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_demand_orderClass_OrderClassId",
                table: "demand",
                column: "OrderClassId",
                principalTable: "orderClass",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_demand_orderClass_OrderClassId",
                table: "demand");

            migrationBuilder.DropIndex(
                name: "IX_demand_OrderClassId",
                table: "demand");

            migrationBuilder.DropColumn(
                name: "OrderClassId",
                table: "demand");

            migrationBuilder.AddColumn<string>(
                name: "DemandId",
                table: "orderClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderClass_DemandId",
                table: "orderClass",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderClass_demand_DemandId",
                table: "orderClass",
                column: "DemandId",
                principalTable: "demand",
                principalColumn: "Id");
        }
    }
}
