using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveReportIdFromOrderBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderByReports_NewReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "NewReportId",
                table: "OrderByReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewReportId",
                table: "OrderByReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports",
                column: "NewReportId");

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
