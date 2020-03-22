using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class alterMenuRevmFileds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HaveDuplicate",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IsPopup",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Menus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HaveDuplicate",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPopup",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
