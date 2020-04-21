using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ReportsTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AxSeries_LineCharts_LineChartId",
                table: "AxSeries");

            migrationBuilder.DropIndex(
                name: "IX_AxSeries_LineChartId",
                table: "AxSeries");

            migrationBuilder.DropColumn(
                name: "LineChartId",
                table: "AxSeries");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AxCharts_ReportId",
                table: "AxCharts",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AxCharts_Reports_ReportId",
                table: "AxCharts",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AxCharts_Reports_ReportId",
                table: "AxCharts");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_AxCharts_ReportId",
                table: "AxCharts");

            migrationBuilder.AddColumn<int>(
                name: "LineChartId",
                table: "AxSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AxSeries_LineChartId",
                table: "AxSeries",
                column: "LineChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AxSeries_LineCharts_LineChartId",
                table: "AxSeries",
                column: "LineChartId",
                principalTable: "LineCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
