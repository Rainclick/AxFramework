using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class rowversionaddedtoAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Menus");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "UserTokens",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "UserSettings",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "UserGroups",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Permissions",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Menus",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Logs",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "LoginLogs",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ConfigData",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AxGroups",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Addresses",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "LoginLogs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ConfigData");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AxGroups");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
