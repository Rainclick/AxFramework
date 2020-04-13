using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class testMigrationTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AxSeries_PieCharts_PieChartId",
                table: "AxSeries");

            migrationBuilder.DropIndex(
                name: "IX_AxSeries_PieChartId",
                table: "AxSeries");

            migrationBuilder.DropColumn(
                name: "PieChartId",
                table: "AxSeries");

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "PieCharts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PieCharts_SeriesId",
                table: "PieCharts",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts",
                column: "SeriesId",
                principalTable: "AxSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts");

            migrationBuilder.DropIndex(
                name: "IX_PieCharts_SeriesId",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "PieCharts");

            migrationBuilder.AddColumn<int>(
                name: "PieChartId",
                table: "AxSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AxSeries_PieChartId",
                table: "AxSeries",
                column: "PieChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AxSeries_PieCharts_PieChartId",
                table: "AxSeries",
                column: "PieChartId",
                principalTable: "PieCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
