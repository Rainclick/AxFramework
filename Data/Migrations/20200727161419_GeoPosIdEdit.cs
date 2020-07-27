using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class GeoPosIdEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geo",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "GeoId",
                table: "Addresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_GeoId",
                table: "Addresses",
                column: "GeoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Geos_GeoId",
                table: "Addresses",
                column: "GeoId",
                principalTable: "Geos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Geos_GeoId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_GeoId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "GeoId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "Geo",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
