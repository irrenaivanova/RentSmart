#nullable disable
namespace RentSmart.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class FixedPropertyLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenterLikes_Renters_RenterId",
                table: "RenterLikes");

            migrationBuilder.RenameColumn(
                name: "RenterId",
                table: "RenterLikes",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RenterLikes_AspNetUsers_UserId",
                table: "RenterLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenterLikes_AspNetUsers_UserId",
                table: "RenterLikes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RenterLikes",
                newName: "RenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RenterLikes_Renters_RenterId",
                table: "RenterLikes",
                column: "RenterId",
                principalTable: "Renters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
