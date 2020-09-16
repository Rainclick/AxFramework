using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class withoneFixed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductInstanceHistories_OpId",
                table: "ProductInstanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_ProductInstanceHistories_PersonnelId",
                table: "ProductInstanceHistories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstanceHistories_OpId",
                table: "ProductInstanceHistories",
                column: "OpId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstanceHistories_PersonnelId",
                table: "ProductInstanceHistories",
                column: "PersonnelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductInstanceHistories_OpId",
                table: "ProductInstanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_ProductInstanceHistories_PersonnelId",
                table: "ProductInstanceHistories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstanceHistories_OpId",
                table: "ProductInstanceHistories",
                column: "OpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInstanceHistories_PersonnelId",
                table: "ProductInstanceHistories",
                column: "PersonnelId",
                unique: true);
        }
    }
}
