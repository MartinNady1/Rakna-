using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class Histories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "CollectedPaymentHistories",
                columns: table => new
                {
                    CollectedPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectedPaymentAmount = table.Column<double>(type: "float", nullable: false),
                    ParkingSessionId = table.Column<int>(type: "int", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ComplaintHistories",
                columns: table => new
                {
                    ComplaintId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplainantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplainantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolvedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplaintReceiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintHistories", x => x.ComplaintId);
                    table.ForeignKey(
                        name: "FK_ComplaintHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSessionHistories",
                columns: table => new
                {
                    ParkingSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    GarageId = table.Column<int>(type: "int", nullable: false),
                    EnterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourlyPrice = table.Column<double>(type: "float", nullable: false),
                    RequiredPayment = table.Column<double>(type: "float", nullable: false),
                    ActualPayment = table.Column<double>(type: "float", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSessionHistories", x => x.ParkingSessionId);
                    table.ForeignKey(
                        name: "FK_ParkingSessionHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationHistories",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GarageId = table.Column<int>(type: "int", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedOrNot = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationHistories", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_ReservationHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffSalaryHistories",
                columns: table => new
                {
                    StaffSalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSalaryHistories", x => x.StaffSalaryId);
                    table.ForeignKey(
                        name: "FK_StaffSalaryHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectedPaymentHistories_HistoryId",
                table: "CollectedPaymentHistories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintHistories_HistoryId",
                table: "ComplaintHistories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSessionHistories_HistoryId",
                table: "ParkingSessionHistories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationHistories_HistoryId",
                table: "ReservationHistories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalaryHistories_HistoryId",
                table: "StaffSalaryHistories",
                column: "HistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectedPaymentHistories");

            migrationBuilder.DropTable(
                name: "ComplaintHistories");

            migrationBuilder.DropTable(
                name: "ParkingSessionHistories");

            migrationBuilder.DropTable(
                name: "ReservationHistories");

            migrationBuilder.DropTable(
                name: "StaffSalaryHistories");

            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
