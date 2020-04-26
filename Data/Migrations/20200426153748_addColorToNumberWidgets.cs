using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addColorToNumberWidgets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legends_PieCharts_PieChartId",
                table: "Legends");

            migrationBuilder.DropIndex(
                name: "IX_Legends_PieChartId",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "PieChartId",
                table: "Legends");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "NumericWidgets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "NumericWidgets");

            migrationBuilder.AddColumn<int>(
                name: "PieChartId",
                table: "Legends",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legends_PieChartId",
                table: "Legends",
                column: "PieChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legends_PieCharts_PieChartId",
                table: "Legends",
                column: "PieChartId",
                principalTable: "PieCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
