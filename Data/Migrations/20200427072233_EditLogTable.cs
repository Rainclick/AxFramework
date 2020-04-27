using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class EditLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoteAddress",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Logs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Logs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "RemoteAddress",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
