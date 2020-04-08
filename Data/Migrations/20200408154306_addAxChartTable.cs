using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addAxChartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarCharts_Menus_SystemId",
                table: "BarCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_Menus_SystemId",
                table: "PieCharts");

            migrationBuilder.DropIndex(
                name: "IX_PieCharts_SystemId",
                table: "PieCharts");

            migrationBuilder.DropIndex(
                name: "IX_BarCharts_SystemId",
                table: "BarCharts");

            migrationBuilder.DropColumn(
                name: "ChartId",
                table: "UserCharts");

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

            migrationBuilder.DropColumn(
                name: "Active",
                table: "BarCharts");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "BarCharts");

            migrationBuilder.DropColumn(
                name: "SystemId",
                table: "BarCharts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BarCharts");

            migrationBuilder.AddColumn<int>(
                name: "AxChartId",
                table: "UserCharts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AxChartId",
                table: "PieCharts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AxChartId",
                table: "BarCharts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AxCharts",
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
                    SystemId = table.Column<int>(nullable: true),
                    ReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AxCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AxCharts_Menus_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCharts_AxChartId",
                table: "UserCharts",
                column: "AxChartId");

            migrationBuilder.CreateIndex(
                name: "IX_PieCharts_AxChartId",
                table: "PieCharts",
                column: "AxChartId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCharts_AxChartId",
                table: "BarCharts",
                column: "AxChartId");

            migrationBuilder.CreateIndex(
                name: "IX_AxCharts_SystemId",
                table: "AxCharts",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BarCharts_AxCharts_AxChartId",
                table: "BarCharts",
                column: "AxChartId",
                principalTable: "AxCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_AxCharts_AxChartId",
                table: "PieCharts",
                column: "AxChartId",
                principalTable: "AxCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharts_AxCharts_AxChartId",
                table: "UserCharts",
                column: "AxChartId",
                principalTable: "AxCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarCharts_AxCharts_AxChartId",
                table: "BarCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_PieCharts_AxCharts_AxChartId",
                table: "PieCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCharts_AxCharts_AxChartId",
                table: "UserCharts");

            migrationBuilder.DropTable(
                name: "AxCharts");

            migrationBuilder.DropIndex(
                name: "IX_UserCharts_AxChartId",
                table: "UserCharts");

            migrationBuilder.DropIndex(
                name: "IX_PieCharts_AxChartId",
                table: "PieCharts");

            migrationBuilder.DropIndex(
                name: "IX_BarCharts_AxChartId",
                table: "BarCharts");

            migrationBuilder.DropColumn(
                name: "AxChartId",
                table: "UserCharts");

            migrationBuilder.DropColumn(
                name: "AxChartId",
                table: "PieCharts");

            migrationBuilder.DropColumn(
                name: "AxChartId",
                table: "BarCharts");

            migrationBuilder.AddColumn<int>(
                name: "ChartId",
                table: "UserCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PieCharts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "PieCharts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemId",
                table: "PieCharts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PieCharts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BarCharts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "BarCharts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemId",
                table: "BarCharts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BarCharts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PieCharts_SystemId",
                table: "PieCharts",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCharts_SystemId",
                table: "BarCharts",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BarCharts_Menus_SystemId",
                table: "BarCharts",
                column: "SystemId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PieCharts_Menus_SystemId",
                table: "PieCharts",
                column: "SystemId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
