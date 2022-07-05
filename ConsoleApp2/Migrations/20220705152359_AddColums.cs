using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp2.Migrations
{
    public partial class AddColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meta",
                table: "counterPartyClass",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meta",
                table: "counterPartyClass");
        }
    }
}
