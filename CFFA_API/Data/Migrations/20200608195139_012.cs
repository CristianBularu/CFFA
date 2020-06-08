using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "Sketch");

            migrationBuilder.DropColumn(
                name: "PageHeight",
                table: "Sketch");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Sketch");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 330, DateTimeKind.Utc).AddTicks(4777),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 809, DateTimeKind.Utc).AddTicks(3403));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 333, DateTimeKind.Utc).AddTicks(5901),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 812, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 322, DateTimeKind.Utc).AddTicks(6355),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 803, DateTimeKind.Utc).AddTicks(4833));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "Sketch",
                type: "int",
                maxLength: 4000,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "PageHeight",
                table: "Sketch",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Sketch",
                type: "nvarchar(89)",
                maxLength: 89,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 809, DateTimeKind.Utc).AddTicks(3403),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 330, DateTimeKind.Utc).AddTicks(4777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 812, DateTimeKind.Utc).AddTicks(3011),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 333, DateTimeKind.Utc).AddTicks(5901));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 803, DateTimeKind.Utc).AddTicks(4833),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 322, DateTimeKind.Utc).AddTicks(6355));
        }
    }
}
