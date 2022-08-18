using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class CreatedDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assortmentClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityStock = table.Column<double>(type: "float", nullable: true),
                    QuantityAllStok = table.Column<double>(type: "float", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assortmentClass", x => x.Id);
                });

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
                name: "salesReturnClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesReturnClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "priceTypeClass",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssortmentClassId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priceTypeClass", x => x.id);
                    table.ForeignKey(
                        name: "FK_priceTypeClass_assortmentClass_AssortmentClassId",
                        column: x => x.AssortmentClassId,
                        principalTable: "assortmentClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "userBaskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterPartyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssortmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userBaskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userBaskets_assortmentClass_AssortmentId",
                        column: x => x.AssortmentId,
                        principalTable: "assortmentClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userBaskets_counterPartyClass_CounterPartyId",
                        column: x => x.CounterPartyId,
                        principalTable: "counterPartyClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "demand",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<long>(type: "bigint", nullable: true),
                    SalesReturnId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_demand_salesReturnClass_SalesReturnId",
                        column: x => x.SalesReturnId,
                        principalTable: "salesReturnClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orderClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateСreation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sum = table.Column<long>(type: "bigint", nullable: false),
                    CounterPartyClassId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DemandId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                        column: x => x.CounterPartyClassId,
                        principalTable: "counterPartyClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderClass_demand_DemandId",
                        column: x => x.DemandId,
                        principalTable: "demand",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "positionClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priceOldOrder = table.Column<long>(type: "bigint", nullable: true),
                    OldQuantity = table.Column<double>(type: "float", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_demand_SalesReturnId",
                table: "demand",
                column: "SalesReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_orderClass_CounterPartyClassId",
                table: "orderClass",
                column: "CounterPartyClassId");

            migrationBuilder.CreateIndex(
                name: "IX_orderClass_DemandId",
                table: "orderClass",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_positionClass_OrderClassId",
                table: "positionClass",
                column: "OrderClassId");

            migrationBuilder.CreateIndex(
                name: "IX_priceTypeClass_AssortmentClassId",
                table: "priceTypeClass",
                column: "AssortmentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_userBaskets_AssortmentId",
                table: "userBaskets",
                column: "AssortmentId");

            migrationBuilder.CreateIndex(
                name: "IX_userBaskets_CounterPartyId",
                table: "userBaskets",
                column: "CounterPartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "positionClass");

            migrationBuilder.DropTable(
                name: "priceTypeClass");

            migrationBuilder.DropTable(
                name: "userBaskets");

            migrationBuilder.DropTable(
                name: "orderClass");

            migrationBuilder.DropTable(
                name: "assortmentClass");

            migrationBuilder.DropTable(
                name: "counterPartyClass");

            migrationBuilder.DropTable(
                name: "demand");

            migrationBuilder.DropTable(
                name: "salesReturnClass");
        }
    }
}
