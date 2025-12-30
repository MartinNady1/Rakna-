using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_vehicles_VehicleID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VehicleID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "DriverID",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_DriverID",
                table: "Reservations",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Drivers_DriverID",
                table: "Reservations",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Drivers_DriverID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_DriverID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "VehicleID",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VehicleID",
                table: "Reservations",
                column: "VehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_vehicles_VehicleID",
                table: "Reservations",
                column: "VehicleID",
                principalTable: "vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
