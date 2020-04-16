using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 955, DateTimeKind.Utc).AddTicks(5368),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 516, DateTimeKind.Utc).AddTicks(5957));

            migrationBuilder.AlterColumn<string>(
                name: "ResetPasswordTokenValue",
                table: "CustomTokens",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationTokenValue",
                table: "CustomTokens",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 958, DateTimeKind.Utc).AddTicks(2228),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 519, DateTimeKind.Utc).AddTicks(706));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 949, DateTimeKind.Utc).AddTicks(4482),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 511, DateTimeKind.Utc).AddTicks(1300));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 516, DateTimeKind.Utc).AddTicks(5957),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 955, DateTimeKind.Utc).AddTicks(5368));

            migrationBuilder.AlterColumn<int>(
                name: "ResetPasswordTokenValue",
                table: "CustomTokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConfirmationTokenValue",
                table: "CustomTokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 519, DateTimeKind.Utc).AddTicks(706),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 958, DateTimeKind.Utc).AddTicks(2228));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 511, DateTimeKind.Utc).AddTicks(1300),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 949, DateTimeKind.Utc).AddTicks(4482));
        }
    }
}
