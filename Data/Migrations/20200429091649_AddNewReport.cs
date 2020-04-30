using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddNewReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    TakeSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ColumnReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FieldId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NewReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColumnReports_NewReports_NewReportId",
                        column: x => x.NewReportId,
                        principalTable: "NewReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderByReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    OrderIndex = table.Column<int>(nullable: false),
                    Column = table.Column<string>(nullable: true),
                    OrderByType = table.Column<int>(nullable: false),
                    NewReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderByReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderByReports_NewReports_NewReportId",
                        column: x => x.NewReportId,
                        principalTable: "NewReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColumnReports_NewReportId",
                table: "ColumnReports",
                column: "NewReportId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderByReports_NewReportId",
                table: "OrderByReports",
                column: "NewReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColumnReports");

            migrationBuilder.DropTable(
                name: "OrderByReports");

            migrationBuilder.DropTable(
                name: "NewReports");
        }
    }
}
