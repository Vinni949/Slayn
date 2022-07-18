using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class AddUserBasketClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OldQuantity",
                table: "positionClass",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "userBaskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterPartyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userBaskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userBaskets_counterPartyClass_CounterPartyId",
                        column: x => x.CounterPartyId,
                        principalTable: "counterPartyClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userBaskets_positionClass_PositionId",
                        column: x => x.PositionId,
                        principalTable: "positionClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userBaskets_CounterPartyId",
                table: "userBaskets",
                column: "CounterPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_userBaskets_PositionId",
                table: "userBaskets",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userBaskets");

            migrationBuilder.AlterColumn<int>(
                name: "OldQuantity",
                table: "positionClass",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
