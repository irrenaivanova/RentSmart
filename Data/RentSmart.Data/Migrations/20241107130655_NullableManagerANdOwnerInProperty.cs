#nullable disable

namespace RentSmart.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class NullableManagerANdOwnerInProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Owners_OwnerId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Renters_UserId",
                table: "Renters");

            migrationBuilder.DropIndex(
                name: "IX_Owners_UserId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Managers_UserId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "DateOfBuying",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Renters_UserId",
                table: "Renters",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Owners_UserId",
                table: "Owners",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                table: "Properties",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Owners_OwnerId",
                table: "Properties",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Owners_OwnerId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Renters_UserId",
                table: "Renters");

            migrationBuilder.DropIndex(
                name: "IX_Owners_UserId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Managers_UserId",
                table: "Managers");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBuying",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Renters_UserId",
                table: "Renters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_UserId",
                table: "Owners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                table: "Properties",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Owners_OwnerId",
                table: "Properties",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
