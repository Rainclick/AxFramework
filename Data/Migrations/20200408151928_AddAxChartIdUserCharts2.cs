using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddAxChartIdUserCharts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PieCharts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "PieCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemId",
                table: "PieCharts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PieCharts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PieCharts_SystemId",
                table: "PieCharts",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_Menus_SystemId",
                table: "PieCharts",
                column: "SystemId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_Menus_SystemId",
                table: "PieCharts");

            migrationBuilder.DropIndex(
                name: "IX_PieCharts_SystemId",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "SystemId",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PieCharts");
        }
    }
}
