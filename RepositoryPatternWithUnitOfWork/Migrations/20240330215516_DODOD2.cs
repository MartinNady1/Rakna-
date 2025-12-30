using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class DODOD2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "parkingSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "reserved",
                table: "ParkingSessionHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "parkingSessions");

            migrationBuilder.DropColumn(
                name: "reserved",
                table: "ParkingSessionHistories");
        }
    }
}
