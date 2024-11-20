using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentSmart.Data.Migrations
{
    /// <inheritdoc />
    public partial class BaseModelToUserLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserLikes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserLikes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UserLikes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_UserId",
                table: "UserLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes");

            migrationBuilder.DropIndex(
                name: "IX_UserLikes_UserId",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikes",
                table: "UserLikes",
                columns: new[] { "UserId", "PropertyId" });
        }
    }
}
