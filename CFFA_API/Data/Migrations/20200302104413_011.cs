using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Sketch",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 809, DateTimeKind.Utc).AddTicks(3403),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 830, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 812, DateTimeKind.Utc).AddTicks(3011),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 833, DateTimeKind.Utc).AddTicks(1229));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 803, DateTimeKind.Utc).AddTicks(4833),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 824, DateTimeKind.Utc).AddTicks(7159));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Sketch");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 830, DateTimeKind.Utc).AddTicks(3027),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 809, DateTimeKind.Utc).AddTicks(3403));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 833, DateTimeKind.Utc).AddTicks(1229),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 812, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 824, DateTimeKind.Utc).AddTicks(7159),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 2, 10, 44, 12, 803, DateTimeKind.Utc).AddTicks(4833));
        }
    }
}
