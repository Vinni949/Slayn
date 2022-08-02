using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models1.Migrations
{
    public partial class AddDBAssortment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userBaskets_positionClass_PositionId",
                table: "userBaskets");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "userBaskets",
                newName: "AssortmentId");

            migrationBuilder.RenameIndex(
                name: "IX_userBaskets_PositionId",
                table: "userBaskets",
                newName: "IX_userBaskets_AssortmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_userBaskets_assortmentClass_AssortmentId",
                table: "userBaskets",
                column: "AssortmentId",
                principalTable: "assortmentClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userBaskets_assortmentClass_AssortmentId",
                table: "userBaskets");

            migrationBuilder.RenameColumn(
                name: "AssortmentId",
                table: "userBaskets",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_userBaskets_AssortmentId",
                table: "userBaskets",
                newName: "IX_userBaskets_PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_userBaskets_positionClass_PositionId",
                table: "userBaskets",
                column: "PositionId",
                principalTable: "positionClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
