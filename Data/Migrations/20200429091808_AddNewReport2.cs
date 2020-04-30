using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddNewReport2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColumnReports_NewReports_NewReportId",
                table: "ColumnReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_NewReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_ColumnReports_NewReportId",
                table: "ColumnReports");

            migrationBuilder.DropColumn(
                name: "NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "NewReportId",
                table: "ColumnReports");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "OrderByReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "ColumnReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_ReportId",
                table: "OrderByReports",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnReports_ReportId",
                table: "ColumnReports",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColumnReports_NewReports_ReportId",
                table: "ColumnReports",
                column: "ReportId",
                principalTable: "NewReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderByReports_NewReports_ReportId",
                table: "OrderByReports",
                column: "ReportId",
                principalTable: "NewReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColumnReports_NewReports_ReportId",
                table: "ColumnReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_NewReports_ReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_ReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_ColumnReports_ReportId",
                table: "ColumnReports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "ColumnReports");

            migrationBuilder.AddColumn<int>(
                name: "NewReportId",
                table: "OrderByReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewReportId",
                table: "ColumnReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports",
                column: "NewReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnReports_NewReportId",
                table: "ColumnReports",
                column: "NewReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColumnReports_NewReports_NewReportId",
                table: "ColumnReports",
                column: "NewReportId",
                principalTable: "NewReports",
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
    }
}
