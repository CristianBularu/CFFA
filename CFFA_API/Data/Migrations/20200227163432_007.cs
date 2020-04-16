using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BmpExtension",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BmpPath",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IconExtension",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePath",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 458, DateTimeKind.Utc).AddTicks(8042),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 707, DateTimeKind.Utc).AddTicks(8885));

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Posts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 461, DateTimeKind.Utc).AddTicks(1400),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 710, DateTimeKind.Utc).AddTicks(4795));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 453, DateTimeKind.Utc).AddTicks(2312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 702, DateTimeKind.Utc).AddTicks(6047));

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 707, DateTimeKind.Utc).AddTicks(8885),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 458, DateTimeKind.Utc).AddTicks(8042));

            migrationBuilder.AddColumn<string>(
                name: "BmpExtension",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BmpPath",
                table: "Posts",
                type: "nvarchar(244)",
                maxLength: 244,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 710, DateTimeKind.Utc).AddTicks(4795),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 461, DateTimeKind.Utc).AddTicks(1400));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 702, DateTimeKind.Utc).AddTicks(6047),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 453, DateTimeKind.Utc).AddTicks(2312));

            migrationBuilder.AddColumn<string>(
                name: "IconExtension",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePath",
                table: "AspNetUsers",
                type: "nvarchar(134)",
                maxLength: 134,
                nullable: true);
        }
    }
}
