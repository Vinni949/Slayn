using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class CreationDemand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_demand_salesReturnClass_SalesReturnId",
                table: "demand");

            migrationBuilder.DropIndex(
                name: "IX_demand_SalesReturnId",
                table: "demand");

            migrationBuilder.DropColumn(
                name: "SalesReturnId",
                table: "demand");

            migrationBuilder.AddColumn<string>(
                name: "DemandId",
                table: "salesReturnClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_salesReturnClass_DemandId",
                table: "salesReturnClass",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_salesReturnClass_demand_DemandId",
                table: "salesReturnClass",
                column: "DemandId",
                principalTable: "demand",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnClass_demand_DemandId",
                table: "salesReturnClass");

            migrationBuilder.DropIndex(
                name: "IX_salesReturnClass_DemandId",
                table: "salesReturnClass");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "salesReturnClass");

            migrationBuilder.AddColumn<string>(
                name: "SalesReturnId",
                table: "demand",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_demand_SalesReturnId",
                table: "demand",
                column: "SalesReturnId");

            migrationBuilder.AddForeignKey(
                name: "FK_demand_salesReturnClass_SalesReturnId",
                table: "demand",
                column: "SalesReturnId",
                principalTable: "salesReturnClass",
                principalColumn: "Id");
        }
    }
}
