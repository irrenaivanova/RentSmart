#nullable disable

namespace RentSmart.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Compudetcolumntofloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Managers_ManagerId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "FeedbackText",
                table: "Feedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "Ratings",
                type: "float",
                nullable: false,
                computedColumnSql: "CAST((ConditionAndMaintenanceRate + Location + ValueForMoney) / 3.0 AS FLOAT)",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "((ConditionAndMaintenanceRate + Location + ValueForMoney) / 3.0)");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Managers_ManagerId",
                table: "Appointments",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Managers_ManagerId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "FeedbackText",
                table: "Feedbacks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "Ratings",
                type: "float",
                nullable: false,
                computedColumnSql: "((ConditionAndMaintenanceRate + Location + ValueForMoney) / 3.0)",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "CAST((ConditionAndMaintenanceRate + Location + ValueForMoney) / 3.0 AS FLOAT)");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Managers_ManagerId",
                table: "Appointments",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }
    }
}
