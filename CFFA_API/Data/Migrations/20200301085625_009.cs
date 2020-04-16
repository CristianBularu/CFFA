using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "PostVoters",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 869, DateTimeKind.Utc).AddTicks(4571),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 341, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 872, DateTimeKind.Utc).AddTicks(9792),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 345, DateTimeKind.Utc).AddTicks(1126));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 862, DateTimeKind.Utc).AddTicks(1917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 335, DateTimeKind.Utc).AddTicks(3775));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "PostVoters",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 341, DateTimeKind.Utc).AddTicks(9844),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 869, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 345, DateTimeKind.Utc).AddTicks(1126),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 872, DateTimeKind.Utc).AddTicks(9792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 335, DateTimeKind.Utc).AddTicks(3775),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 862, DateTimeKind.Utc).AddTicks(1917));
        }
    }
}
