using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class DODOD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "ParkingSessionHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PlateLetters",
                table: "ParkingSessionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumbers",
                table: "ParkingSessionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateLetters",
                table: "ParkingSessionHistories");

            migrationBuilder.DropColumn(
                name: "PlateNumbers",
                table: "ParkingSessionHistories");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "ParkingSessionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplaintId",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
