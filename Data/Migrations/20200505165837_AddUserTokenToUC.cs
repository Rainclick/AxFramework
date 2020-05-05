using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddUserTokenToUC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_Users_UserId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections");

            migrationBuilder.AddColumn<int>(
                name: "UserTokenId",
                table: "UserConnections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_UserTokenId",
                table: "UserConnections",
                column: "UserTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_UserTokens_UserTokenId",
                table: "UserConnections",
                column: "UserTokenId",
                principalTable: "UserTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_UserTokens_UserTokenId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_UserTokenId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "UserTokenId",
                table: "UserConnections");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_Users_UserId",
                table: "UserConnections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
