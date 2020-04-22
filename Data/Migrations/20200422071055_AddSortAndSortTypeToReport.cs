using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddSortAndSortTypeToReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sort",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortType",
                table: "Reports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SortType",
                table: "Reports");
        }
    }
}
