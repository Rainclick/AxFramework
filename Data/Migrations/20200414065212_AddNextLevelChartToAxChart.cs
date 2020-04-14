using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddNextLevelChartToAxChart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextChartId",
                table: "AxCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextChartType",
                table: "AxCharts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextChartId",
                table: "AxCharts");

            migrationBuilder.DropColumn(
                name: "NextChartType",
                table: "AxCharts");
        }
    }
}
