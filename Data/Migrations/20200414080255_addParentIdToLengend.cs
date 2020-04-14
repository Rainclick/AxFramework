using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addParentIdToLengend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Legends");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Legends",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Legends");

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "Legends",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
