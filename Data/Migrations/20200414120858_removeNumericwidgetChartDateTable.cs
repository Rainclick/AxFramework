using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class removeNumericwidgetChartDateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "NumericWidgets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdated",
                table: "NumericWidgets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
