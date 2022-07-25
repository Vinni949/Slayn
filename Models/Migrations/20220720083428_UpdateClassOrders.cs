using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class UpdateClassOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                table: "orderClass");

            migrationBuilder.AlterColumn<long>(
                name: "priceOldOrder",
                table: "positionClass",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "sum",
                table: "orderClass",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CounterPartyClassId",
                table: "orderClass",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                table: "orderClass",
                column: "CounterPartyClassId",
                principalTable: "counterPartyClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                table: "orderClass");

            migrationBuilder.AlterColumn<string>(
                name: "priceOldOrder",
                table: "positionClass",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sum",
                table: "orderClass",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "CounterPartyClassId",
                table: "orderClass",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_orderClass_counterPartyClass_CounterPartyClassId",
                table: "orderClass",
                column: "CounterPartyClassId",
                principalTable: "counterPartyClass",
                principalColumn: "Id");
        }
    }
}
