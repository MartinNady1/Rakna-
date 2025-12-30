using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class totomama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "StaffSalaryHistories",
                newName: "StaffName");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "StaffSalaryHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaffEmail",
                table: "StaffSalaryHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfJoining",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalaryHistories_EmployeeId",
                table: "StaffSalaryHistories",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSalaryHistories_Employees_EmployeeId",
                table: "StaffSalaryHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffSalaryHistories_Employees_EmployeeId",
                table: "StaffSalaryHistories");

            migrationBuilder.DropIndex(
                name: "IX_StaffSalaryHistories_EmployeeId",
                table: "StaffSalaryHistories");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "StaffSalaryHistories");

            migrationBuilder.DropColumn(
                name: "StaffEmail",
                table: "StaffSalaryHistories");

            migrationBuilder.DropColumn(
                name: "DateOfJoining",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "StaffName",
                table: "StaffSalaryHistories",
                newName: "StaffId");
        }
    }
}
