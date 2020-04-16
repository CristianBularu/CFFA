using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CFFA_API.Data.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoritePosts_Posts_PostId",
                table: "UserFavoritePosts");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "UserFavoritePosts",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 516, DateTimeKind.Utc).AddTicks(5957),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 803, DateTimeKind.Utc).AddTicks(2050));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 519, DateTimeKind.Utc).AddTicks(706),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 807, DateTimeKind.Utc).AddTicks(3759));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 511, DateTimeKind.Utc).AddTicks(1300),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 801, DateTimeKind.Utc).AddTicks(3693));

            migrationBuilder.AddColumn<int>(
                name: "CustomTokensId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmationTokenValue = table.Column<int>(nullable: false),
                    ConfirmationTokenCreationTime = table.Column<DateTime>(nullable: false),
                    ConfirmationTokenAttempts = table.Column<int>(nullable: false),
                    ResetPasswordTokenValue = table.Column<int>(nullable: false),
                    ResetPasswordCreationTime = table.Column<DateTime>(nullable: false),
                    ResetPasswordAttempts = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomTokens_UserId",
                table: "CustomTokens",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoritePosts_Posts_PostId",
                table: "UserFavoritePosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoritePosts_Posts_PostId",
                table: "UserFavoritePosts");

            migrationBuilder.DropTable(
                name: "CustomTokens");

            migrationBuilder.DropColumn(
                name: "CustomTokensId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "UserFavoritePosts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 803, DateTimeKind.Utc).AddTicks(2050),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 516, DateTimeKind.Utc).AddTicks(5957));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 807, DateTimeKind.Utc).AddTicks(3759),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 519, DateTimeKind.Utc).AddTicks(706));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 16, 21, 16, 48, 801, DateTimeKind.Utc).AddTicks(3693),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 2, 2, 10, 54, 48, 511, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoritePosts_Posts_PostId",
                table: "UserFavoritePosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
