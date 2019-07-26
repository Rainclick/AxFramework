using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    GenderType = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
