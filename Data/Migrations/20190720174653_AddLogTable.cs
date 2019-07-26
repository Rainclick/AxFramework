using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    Logged = table.Column<DateTime>(nullable: false),
                    Level = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    ServerName = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ServerAddress = table.Column<string>(nullable: true),
                    RemoteAddress = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Callsite = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
