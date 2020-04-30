using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddTargetTypeCoulmn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "OrderByReports");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "ColumnReports");

            migrationBuilder.AddColumn<int>(
                name: "ColumnType",
                table: "ColumnReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JoinId",
                table: "ColumnReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "ColumnReports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnType",
                table: "ColumnReports");

            migrationBuilder.DropColumn(
                name: "JoinId",
                table: "ColumnReports");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "ColumnReports");

            migrationBuilder.AddColumn<string>(
                name: "Column",
                table: "OrderByReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "ColumnReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
