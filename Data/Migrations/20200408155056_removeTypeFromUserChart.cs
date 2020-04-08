using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class removeTypeFromUserChart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserCharts");

            migrationBuilder.AddColumn<int>(
                name: "ChartType",
                table: "AxCharts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChartType",
                table: "AxCharts");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
