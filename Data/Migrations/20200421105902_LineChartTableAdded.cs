using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class LineChartTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineChartId",
                table: "Legends",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineChartId",
                table: "AxSeries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LineCharts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AxChartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineCharts_AxCharts_AxChartId",
                        column: x => x.AxChartId,
                        principalTable: "AxCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legends_LineChartId",
                table: "Legends",
                column: "LineChartId");

            migrationBuilder.CreateIndex(
                name: "IX_AxSeries_LineChartId",
                table: "AxSeries",
                column: "LineChartId");

            migrationBuilder.CreateIndex(
                name: "IX_LineCharts_AxChartId",
                table: "LineCharts",
                column: "AxChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AxSeries_LineCharts_LineChartId",
                table: "AxSeries",
                column: "LineChartId",
                principalTable: "LineCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Legends_LineCharts_LineChartId",
                table: "Legends",
                column: "LineChartId",
                principalTable: "LineCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AxSeries_LineCharts_LineChartId",
                table: "AxSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_Legends_LineCharts_LineChartId",
                table: "Legends");

            migrationBuilder.DropTable(
                name: "LineCharts");

            migrationBuilder.DropIndex(
                name: "IX_Legends_LineChartId",
                table: "Legends");

            migrationBuilder.DropIndex(
                name: "IX_AxSeries_LineChartId",
                table: "AxSeries");

            migrationBuilder.DropColumn(
                name: "LineChartId",
                table: "Legends");

            migrationBuilder.DropColumn(
                name: "LineChartId",
                table: "AxSeries");
        }
    }
}
