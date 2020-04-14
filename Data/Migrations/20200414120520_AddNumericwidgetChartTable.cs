using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddNumericwidgetChartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumericWidgets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<string>(nullable: true),
                    AxChartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumericWidgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumericWidgets_AxCharts_AxChartId",
                        column: x => x.AxChartId,
                        principalTable: "AxCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NumericWidgets_AxChartId",
                table: "NumericWidgets",
                column: "AxChartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumericWidgets");
        }
    }
}
