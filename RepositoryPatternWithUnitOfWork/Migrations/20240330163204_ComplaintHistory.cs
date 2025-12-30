using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rakna.EF.Migrations
{
    public partial class ComplaintHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplaintHistories",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "ComplaintId",
                table: "ComplaintHistories");

            migrationBuilder.AddColumn<int>(
                name: "ComplaintHistoryId",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ComplaintId",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplaintHistories",
                table: "ComplaintHistories",
                column: "ComplaintHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplaintHistories",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "ComplaintHistoryId",
                table: "ComplaintHistories");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Reports");

            // Recreate the ComplaintId column as an identity column, which was its original state
            migrationBuilder.AddColumn<int>(
                name: "ComplaintId",
                table: "ComplaintHistories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplaintHistories",
                table: "ComplaintHistories",
                column: "ComplaintId");
        }
    }
}
