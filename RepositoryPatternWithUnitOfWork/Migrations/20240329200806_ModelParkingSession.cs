using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class ModelParkingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_parkingSessions_vehicles_VehicleID",
                table: "parkingSessions");

            /*migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16473c57-bde4-4014-bad9-cd2cfcbcacaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1688c50d-8afa-475a-b1ed-d11c35f34ea2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38464ea5-bdf4-4ace-989d-f4909d9ebca6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d1cadcd-cc6b-4d43-a626-37a0f7932f66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6501e3c-9c71-416a-bea3-52d667cfefe9");*/

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "parkingSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "parkingSessions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

           /* migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a653f57-cd5a-4262-a12d-d5b03dc749b6", "d0037162-f8b5-4e6c-ae37-54526b8dcb67", "customerservice", "CUSTOMERSERVICE" },
                    { "a0d653b1-3fb6-419c-b4bf-53091ccf155d", "f1c4ed0b-a0e5-4a46-bd2a-108f473848f9", "driver", "DRIVER" },
                    { "b3c78b26-fee0-4baa-ab03-b43e8703cf2a", "a1e01a4b-978e-4445-ae3e-27cc8e6bdc66", "garagestaff", "GARAGESTAFF" },
                    { "b8c952df-03d6-4fd9-85ad-96a068abb3c0", "bfe7f727-42a4-4ea7-9c29-efa6ba172028", "garageadmin", "GARAGEADMIN" },
                    { "ce2bc82c-a864-4b68-bcbf-1c8a3f10d7f3", "fd37334c-4d65-4705-bfee-2bce3104af1f", "technicalsupport", "TECHNICALSUPPORT" }
                });*/

            migrationBuilder.AddForeignKey(
                name: "FK_parkingSessions_vehicles_VehicleID",
                table: "parkingSessions",
                column: "VehicleID",
                principalTable: "vehicles",
                principalColumn: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_parkingSessions_vehicles_VehicleID",
                table: "parkingSessions");
/*
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a653f57-cd5a-4262-a12d-d5b03dc749b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0d653b1-3fb6-419c-b4bf-53091ccf155d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3c78b26-fee0-4baa-ab03-b43e8703cf2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8c952df-03d6-4fd9-85ad-96a068abb3c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce2bc82c-a864-4b68-bcbf-1c8a3f10d7f3");*/

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "parkingSessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "parkingSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

         /*   migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16473c57-bde4-4014-bad9-cd2cfcbcacaf", "632b7c4f-7d6f-47bf-9e7b-bd2794e86353", "customerservice", "CUSTOMERSERVICE" },
                    { "1688c50d-8afa-475a-b1ed-d11c35f34ea2", "85ebd8d9-ce65-4e12-a339-2bc8835e3aa2", "technicalsupport", "TECHNICALSUPPORT" },
                    { "38464ea5-bdf4-4ace-989d-f4909d9ebca6", "b8ded986-4a9d-4ca9-8907-03a877f81e5f", "garageadmin", "GARAGEADMIN" },
                    { "3d1cadcd-cc6b-4d43-a626-37a0f7932f66", "bfed1dfc-603c-4332-b44e-7c8a1801095d", "driver", "DRIVER" },
                    { "b6501e3c-9c71-416a-bea3-52d667cfefe9", "a0336c20-2a24-40c8-8086-37d0cb34ff4d", "garagestaff", "GARAGESTAFF" }
                });
*/
            migrationBuilder.AddForeignKey(
                name: "FK_parkingSessions_vehicles_VehicleID",
                table: "parkingSessions",
                column: "VehicleID",
                principalTable: "vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
