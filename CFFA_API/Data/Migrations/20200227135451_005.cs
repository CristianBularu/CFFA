using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoters_Posts_PostId",
                table: "PostVoters");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "PostVoters",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "PostVoters",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 415, DateTimeKind.Utc).AddTicks(165),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 994, DateTimeKind.Utc).AddTicks(716));

            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "CommentVoters",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 417, DateTimeKind.Utc).AddTicks(3855),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 996, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.AlterColumn<string>(
                name: "IconPath",
                table: "AspNetUsers",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(89)",
                oldMaxLength: 89,
                oldDefaultValue: "\\uploadedfiles\\profiles\\0.png");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 409, DateTimeKind.Utc).AddTicks(5429),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 988, DateTimeKind.Utc).AddTicks(5990));

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoters_Posts_PostId",
                table: "PostVoters",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoters_Posts_PostId",
                table: "PostVoters");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "PostVoters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "PostVoters",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 994, DateTimeKind.Utc).AddTicks(716),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 415, DateTimeKind.Utc).AddTicks(165));

            migrationBuilder.AlterColumn<bool>(
                name: "PositiveVot",
                table: "CommentVoters",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 996, DateTimeKind.Utc).AddTicks(5035),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 417, DateTimeKind.Utc).AddTicks(3855));

            migrationBuilder.AlterColumn<string>(
                name: "IconPath",
                table: "AspNetUsers",
                type: "nvarchar(89)",
                maxLength: 89,
                nullable: false,
                defaultValue: "\\uploadedfiles\\profiles\\0.png",
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 6, 10, 52, 48, 988, DateTimeKind.Utc).AddTicks(5990),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 27, 13, 54, 51, 409, DateTimeKind.Utc).AddTicks(5429));

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoters_Posts_PostId",
                table: "PostVoters",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
