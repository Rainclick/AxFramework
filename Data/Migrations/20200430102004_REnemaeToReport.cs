using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class REnemaeToReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AxCharts_RS_ReportId",
                table: "AxCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_AxFilters_RS_ReportId",
                table: "AxFilters");

            migrationBuilder.DropTable(
                name: "RS");

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
                    Title = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    ResultTypeName = table.Column<string>(nullable: true),
                    TakeSize = table.Column<int>(nullable: false),
                    Sort = table.Column<string>(nullable: true),
                    SortType = table.Column<int>(nullable: false),
                    ExecuteType = table.Column<int>(nullable: false),
                    GroupBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AxCharts_Reports_ReportId",
                table: "AxCharts",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AxFilters_Reports_ReportId",
                table: "AxFilters",
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

            migrationBuilder.DropForeignKey(
                name: "FK_AxFilters_Reports_ReportId",
                table: "AxFilters");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.CreateTable(
                name: "RS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false),
                    ExecuteType = table.Column<int>(type: "int", nullable: false),
                    GroupBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifierUserId = table.Column<int>(type: "int", nullable: true),
                    ResultTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Sort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortType = table.Column<int>(type: "int", nullable: false),
                    TakeSize = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RS", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AxCharts_RS_ReportId",
                table: "AxCharts",
                column: "ReportId",
                principalTable: "RS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AxFilters_RS_ReportId",
                table: "AxFilters",
                column: "ReportId",
                principalTable: "RS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
