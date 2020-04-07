using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AxChartsTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BarCharts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    XField = table.Column<string>(nullable: true),
                    YField = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    SystemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarCharts_Menus_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PieCharts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    SystemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieCharts_Menus_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AxSeries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    AxChartId = table.Column<int>(nullable: false),
                    BarChartId = table.Column<int>(nullable: true),
                    PieChartId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AxSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AxSeries_BarCharts_BarChartId",
                        column: x => x.BarChartId,
                        principalTable: "BarCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AxSeries_PieCharts_PieChartId",
                        column: x => x.PieChartId,
                        principalTable: "PieCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Legends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AxChartId = table.Column<int>(nullable: false),
                    BarChartId = table.Column<int>(nullable: true),
                    PieChartId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legends_BarCharts_BarChartId",
                        column: x => x.BarChartId,
                        principalTable: "BarCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Legends_PieCharts_PieChartId",
                        column: x => x.PieChartId,
                        principalTable: "PieCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AxSeries_BarChartId",
                table: "AxSeries",
                column: "BarChartId");

            migrationBuilder.CreateIndex(
                name: "IX_AxSeries_PieChartId",
                table: "AxSeries",
                column: "PieChartId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCharts_SystemId",
                table: "BarCharts",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Legends_BarChartId",
                table: "Legends",
                column: "BarChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Legends_PieChartId",
                table: "Legends",
                column: "PieChartId");

            migrationBuilder.CreateIndex(
                name: "IX_PieCharts_SystemId",
                table: "PieCharts",
                column: "SystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AxSeries");

            migrationBuilder.DropTable(
                name: "Legends");

            migrationBuilder.DropTable(
                name: "BarCharts");

            migrationBuilder.DropTable(
                name: "PieCharts");
        }
    }
}
