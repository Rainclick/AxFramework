using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class withoneFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductInstances_PersonnelId",
                table: "ProductInstances");

            migrationBuilder.DropIndex(
                name: "IX_ProductInstances_ProductLineId",
                table: "ProductInstances");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_PersonnelId",
                table: "ProductInstances",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_ProductLineId",
                table: "ProductInstances",
                column: "ProductLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductInstances_PersonnelId",
                table: "ProductInstances");

            migrationBuilder.DropIndex(
                name: "IX_ProductInstances_ProductLineId",
                table: "ProductInstances");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_PersonnelId",
                table: "ProductInstances",
                column: "PersonnelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstances_ProductLineId",
                table: "ProductInstances",
                column: "ProductLineId",
                unique: true);
        }
    }
}
