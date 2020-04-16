using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 341, DateTimeKind.Utc).AddTicks(9844),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 458, DateTimeKind.Utc).AddTicks(8042));

            migrationBuilder.AddColumn<long>(
                name: "SketchId",
                table: "Posts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 345, DateTimeKind.Utc).AddTicks(1126),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 461, DateTimeKind.Utc).AddTicks(1400));

            migrationBuilder.AlterColumn<bool>(
                name: "Premium",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 335, DateTimeKind.Utc).AddTicks(3775),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 453, DateTimeKind.Utc).AddTicks(2312));

            migrationBuilder.CreateTable(
                name: "Sketch",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 89, nullable: false),
                    PageCount = table.Column<int>(maxLength: 4000, nullable: false),
                    PageHeight = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sketch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sketch_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SketchId",
                table: "Posts",
                column: "SketchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sketch_UserId",
                table: "Sketch",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Sketch_SketchId",
                table: "Posts",
                column: "SketchId",
                principalTable: "Sketch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Sketch_SketchId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Sketch");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SketchId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SketchId",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 458, DateTimeKind.Utc).AddTicks(8042),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 341, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 461, DateTimeKind.Utc).AddTicks(1400),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 345, DateTimeKind.Utc).AddTicks(1126));

            migrationBuilder.AlterColumn<bool>(
                name: "Premium",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 27, 16, 34, 32, 453, DateTimeKind.Utc).AddTicks(2312),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 1, 8, 54, 52, 335, DateTimeKind.Utc).AddTicks(3775));
        }
    }
}
