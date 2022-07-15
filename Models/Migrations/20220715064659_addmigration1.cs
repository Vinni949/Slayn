using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class addmigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "counterPartyClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginOfAccessToTheLC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginOfPsswordToTheLC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Meta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counterPartyClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orderClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateСreation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<int>(type: "int", nullable: false),
                    CounterPartyClassId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                        column: x => x.CounterPartyClassId,
                        principalTable: "counterPartyClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "positionClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priceOldOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    OrderClassId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positionClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_positionClass_orderClass_OrderClassId",
                        column: x => x.OrderClassId,
                        principalTable: "orderClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "priceTypeClass",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PositionClassId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priceTypeClass", x => x.id);
                    table.ForeignKey(
                        name: "FK_priceTypeClass_positionClass_PositionClassId",
                        column: x => x.PositionClassId,
                        principalTable: "positionClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderClass_CounterPartyClassId",
                table: "orderClass",
                column: "CounterPartyClassId");

            migrationBuilder.CreateIndex(
                name: "IX_positionClass_OrderClassId",
                table: "positionClass",
                column: "OrderClassId");

            migrationBuilder.CreateIndex(
                name: "IX_priceTypeClass_PositionClassId",
                table: "priceTypeClass",
                column: "PositionClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "priceTypeClass");

            migrationBuilder.DropTable(
                name: "positionClass");

            migrationBuilder.DropTable(
                name: "orderClass");

            migrationBuilder.DropTable(
                name: "counterPartyClass");
        }
    }
}
