using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class AddDemandAndSalesReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "orderClass");

            migrationBuilder.DropColumn(
                name: "DemandName",
                table: "orderClass");

            migrationBuilder.DropColumn(
                name: "Return",
                table: "orderClass");

            migrationBuilder.AddColumn<string>(
                name: "DemandIdId",
                table: "orderClass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalesReturnClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReturnClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demand",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<long>(type: "bigint", nullable: false),
                    SalesReturnId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demand_SalesReturnClass_SalesReturnId",
                        column: x => x.SalesReturnId,
                        principalTable: "SalesReturnClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderClass_DemandIdId",
                table: "orderClass",
                column: "DemandIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_SalesReturnId",
                table: "Demand",
                column: "SalesReturnId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderClass_Demand_DemandIdId",
                table: "orderClass",
                column: "DemandIdId",
                principalTable: "Demand",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderClass_Demand_DemandIdId",
                table: "orderClass");

            migrationBuilder.DropTable(
                name: "Demand");

            migrationBuilder.DropTable(
                name: "SalesReturnClass");

            migrationBuilder.DropIndex(
                name: "IX_orderClass_DemandIdId",
                table: "orderClass");

            migrationBuilder.DropColumn(
                name: "DemandIdId",
                table: "orderClass");

            migrationBuilder.AddColumn<string>(
                name: "DemandId",
                table: "orderClass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DemandName",
                table: "orderClass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Return",
                table: "orderClass",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
