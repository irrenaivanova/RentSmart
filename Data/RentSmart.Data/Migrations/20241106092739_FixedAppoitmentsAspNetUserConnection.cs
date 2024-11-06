#nullable disable

namespace RentSmart.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class FixedAppoitmentsAspNetUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Renters_RenterId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "RenterId",
                table: "Appointments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RenterId",
                table: "Appointments",
                newName: "IX_Appointments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_UserId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Appointments",
                newName: "RenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                newName: "IX_Appointments_RenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Renters_RenterId",
                table: "Appointments",
                column: "RenterId",
                principalTable: "Renters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
