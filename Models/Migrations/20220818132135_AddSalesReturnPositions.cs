using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class AddSalesReturnPositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CounterPartiesId",
                table: "salesReturnClass",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesReturnPositionsId",
                table: "salesReturnClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalesReturnPositionsClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priceOldOrder = table.Column<long>(type: "bigint", nullable: true),
                    OldQuantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReturnPositionsClass", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salesReturnClass_SalesReturnPositionsClass_SalesReturnPositionsId",
                table: "salesReturnClass");

            migrationBuilder.DropTable(
                name: "SalesReturnPositionsClass");

            migrationBuilder.DropIndex(
                name: "IX_salesReturnClass_SalesReturnPositionsId",
                table: "salesReturnClass");

            migrationBuilder.DropColumn(
                name: "CounterPartiesId",
                table: "salesReturnClass");

            migrationBuilder.DropColumn(
                name: "SalesReturnPositionsId",
                table: "salesReturnClass");
        }
    }
}
