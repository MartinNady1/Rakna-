using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCollected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectedPaymentHistories");

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "ParkingSessionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "ParkingSessionHistories");

            migrationBuilder.CreateTable(
                name: "CollectedPaymentHistories",
                columns: table => new
                {
                    CollectedPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: false),
                    CollectedPaymentAmount = table.Column<double>(type: "float", nullable: false),
                    ParkingSessionId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectedPaymentHistories", x => x.CollectedPaymentId);
                    table.ForeignKey(
                        name: "FK_CollectedPaymentHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectedPaymentHistories_HistoryId",
                table: "CollectedPaymentHistories",
                column: "HistoryId");
        }
    }
}
