using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class testMigrationTest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts");

            migrationBuilder.AlterColumn<int>(
                name: "SeriesId",
                table: "PieCharts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts",
                column: "SeriesId",
                principalTable: "AxSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts");

            migrationBuilder.AlterColumn<int>(
                name: "SeriesId",
                table: "PieCharts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_AxSeries_SeriesId",
                table: "PieCharts",
                column: "SeriesId",
                principalTable: "AxSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
