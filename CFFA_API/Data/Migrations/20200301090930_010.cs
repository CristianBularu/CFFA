using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_SketchId",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 830, DateTimeKind.Utc).AddTicks(3027),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 869, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 833, DateTimeKind.Utc).AddTicks(1229),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 872, DateTimeKind.Utc).AddTicks(9792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 824, DateTimeKind.Utc).AddTicks(7159),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 862, DateTimeKind.Utc).AddTicks(1917));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SketchId",
                table: "Posts",
                column: "SketchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_SketchId",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 869, DateTimeKind.Utc).AddTicks(4571),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 830, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 872, DateTimeKind.Utc).AddTicks(9792),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 833, DateTimeKind.Utc).AddTicks(1229));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 56, 24, 862, DateTimeKind.Utc).AddTicks(1917),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 9, 9, 29, 824, DateTimeKind.Utc).AddTicks(7159));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SketchId",
                table: "Posts",
                column: "SketchId",
                unique: true);
        }
    }
}
