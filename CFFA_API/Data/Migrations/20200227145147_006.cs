using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 707, DateTimeKind.Utc).AddTicks(8885),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 415, DateTimeKind.Utc).AddTicks(165));

            migrationBuilder.AddColumn<string>(
                name: "BmpExtension",
                table: "Posts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 710, DateTimeKind.Utc).AddTicks(4795),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 417, DateTimeKind.Utc).AddTicks(3855));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 702, DateTimeKind.Utc).AddTicks(6047),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 409, DateTimeKind.Utc).AddTicks(5429));

            migrationBuilder.AddColumn<string>(
                name: "IconExtension",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BmpExtension",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IconExtension",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 415, DateTimeKind.Utc).AddTicks(165),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 707, DateTimeKind.Utc).AddTicks(8885));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 417, DateTimeKind.Utc).AddTicks(3855),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 710, DateTimeKind.Utc).AddTicks(4795));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 409, DateTimeKind.Utc).AddTicks(5429),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 14, 51, 46, 702, DateTimeKind.Utc).AddTicks(6047));
        }
    }
}
