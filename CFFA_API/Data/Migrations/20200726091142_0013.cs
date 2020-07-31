using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _0013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 120, DateTimeKind.Utc).AddTicks(674),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 330, DateTimeKind.Utc).AddTicks(4777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 121, DateTimeKind.Utc).AddTicks(7993),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 333, DateTimeKind.Utc).AddTicks(5901));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 116, DateTimeKind.Utc).AddTicks(5573),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 322, DateTimeKind.Utc).AddTicks(6355));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 330, DateTimeKind.Utc).AddTicks(4777),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 120, DateTimeKind.Utc).AddTicks(674));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 333, DateTimeKind.Utc).AddTicks(5901),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 121, DateTimeKind.Utc).AddTicks(7993));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 19, 51, 39, 322, DateTimeKind.Utc).AddTicks(6355),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 26, 9, 11, 42, 116, DateTimeKind.Utc).AddTicks(5573));
        }
    }
}
