using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class CreatedDbSetAddSalesReturnPositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnClass_SalesReturnPositionsClass_SalesReturnPositionsId",
                table: "salesReturnClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesReturnPositionsClass",
                table: "SalesReturnPositionsClass");

            migrationBuilder.DropIndex(
                name: "IX_salesReturnClass_SalesReturnPositionsId",
                table: "salesReturnClass");

            migrationBuilder.DropColumn(
                name: "SalesReturnPositionsId",
                table: "salesReturnClass");

            migrationBuilder.RenameTable(
                name: "SalesReturnPositionsClass",
                newName: "salesReturnPositionsClass");

            migrationBuilder.AddColumn<string>(
                name: "SalesReturnClassId",
                table: "salesReturnPositionsClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_salesReturnPositionsClass",
                table: "salesReturnPositionsClass",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_salesReturnPositionsClass_SalesReturnClassId",
                table: "salesReturnPositionsClass",
                column: "SalesReturnClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_salesReturnPositionsClass_salesReturnClass_SalesReturnClassId",
                table: "salesReturnPositionsClass",
                column: "SalesReturnClassId",
                principalTable: "salesReturnClass",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnPositionsClass_salesReturnClass_SalesReturnClassId",
                table: "salesReturnPositionsClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_salesReturnPositionsClass",
                table: "salesReturnPositionsClass");

            migrationBuilder.DropIndex(
                name: "IX_salesReturnPositionsClass_SalesReturnClassId",
                table: "salesReturnPositionsClass");

            migrationBuilder.DropColumn(
                name: "SalesReturnClassId",
                table: "salesReturnPositionsClass");

            migrationBuilder.RenameTable(
                name: "salesReturnPositionsClass",
                newName: "SalesReturnPositionsClass");

            migrationBuilder.AddColumn<string>(
                name: "SalesReturnPositionsId",
                table: "salesReturnClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesReturnPositionsClass",
                table: "SalesReturnPositionsClass",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_salesReturnClass_SalesReturnPositionsId",
                table: "salesReturnClass",
                column: "SalesReturnPositionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_salesReturnClass_SalesReturnPositionsClass_SalesReturnPositionsId",
                table: "salesReturnClass",
                column: "SalesReturnPositionsId",
                principalTable: "SalesReturnPositionsClass",
                principalColumn: "Id");
        }
    }
}
