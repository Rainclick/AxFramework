using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddColumnToOrderBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_NewReports_ReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_ReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "OrderByReports");

            migrationBuilder.AddColumn<int>(
                name: "ColumnReportId",
                table: "OrderByReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewReportId",
                table: "OrderByReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_ColumnReportId",
                table: "OrderByReports",
                column: "ColumnReportId",
                unique: true,
                filter: "[ColumnReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports",
                column: "NewReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderByReports_ColumnReports_ColumnReportId",
                table: "OrderByReports",
                column: "ColumnReportId",
                principalTable: "ColumnReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderByReports_NewReports_NewReportId",
                table: "OrderByReports",
                column: "NewReportId",
                principalTable: "NewReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_ColumnReports_ColumnReportId",
                table: "OrderByReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_NewReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_ColumnReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "ColumnReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "NewReportId",
                table: "OrderByReports");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "OrderByReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_ReportId",
                table: "OrderByReports",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderByReports_NewReports_ReportId",
                table: "OrderByReports",
                column: "ReportId",
                principalTable: "NewReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
