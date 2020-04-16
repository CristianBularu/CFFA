using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 994, DateTimeKind.Utc).AddTicks(716),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 955, DateTimeKind.Utc).AddTicks(5368));

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenAttempts",
                table: "CustomTokens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenCreationTime",
                table: "CustomTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenValue",
                table: "CustomTokens",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 996, DateTimeKind.Utc).AddTicks(5035),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 958, DateTimeKind.Utc).AddTicks(2228));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 988, DateTimeKind.Utc).AddTicks(5990),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 949, DateTimeKind.Utc).AddTicks(4482));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenAttempts",
                table: "CustomTokens");

            migrationBuilder.DropColumn(
                name: "RefreshTokenCreationTime",
                table: "CustomTokens");

            migrationBuilder.DropColumn(
                name: "RefreshTokenValue",
                table: "CustomTokens");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 955, DateTimeKind.Utc).AddTicks(5368),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 994, DateTimeKind.Utc).AddTicks(716));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 958, DateTimeKind.Utc).AddTicks(2228),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 996, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 15, 44, 22, 949, DateTimeKind.Utc).AddTicks(4482),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 988, DateTimeKind.Utc).AddTicks(5990));
        }
    }
}
