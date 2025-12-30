using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class LinkOtp1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailOTPs_Drivers_DriverId",
                table: "EmailOTPs");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "EmailOTPs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailOTPs_DriverId",
                table: "EmailOTPs",
                newName: "IX_EmailOTPs_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "EmailOTPs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OTPType",
                table: "EmailOTPs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailOTPs_AspNetUsers_UserId",
                table: "EmailOTPs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailOTPs_AspNetUsers_UserId",
                table: "EmailOTPs");

            migrationBuilder.DropColumn(
                name: "OTPType",
                table: "EmailOTPs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmailOTPs",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailOTPs_UserId",
                table: "EmailOTPs",
                newName: "IX_EmailOTPs_DriverId");

            migrationBuilder.AlterColumn<int>(
                name: "OTP",
                table: "EmailOTPs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailOTPs_Drivers_DriverId",
                table: "EmailOTPs",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
