using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddFileAttachmentTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileAttachmentTypeId",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileAttachmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ModifierUserId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TypeTitle = table.Column<string>(nullable: true),
                    IsBaseType = table.Column<bool>(nullable: false),
                    AttachmentTypeEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachmentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_FileAttachmentTypeId",
                table: "FileAttachments",
                column: "FileAttachmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_FileAttachmentTypes_FileAttachmentTypeId",
                table: "FileAttachments",
                column: "FileAttachmentTypeId",
                principalTable: "FileAttachmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_FileAttachmentTypes_FileAttachmentTypeId",
                table: "FileAttachments");

            migrationBuilder.DropTable(
                name: "FileAttachmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_FileAttachmentTypeId",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "FileAttachmentTypeId",
                table: "FileAttachments");
        }
    }
}
