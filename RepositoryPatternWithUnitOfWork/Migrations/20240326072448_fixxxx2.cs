using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class fixxxx2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Drivers_DriverID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03db5abf-2a40-427d-9909-1eb555b0168c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4aea9276-7db6-4441-a2bc-54cd9251530b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58abd4cb-ef6f-4488-ad73-5e5c944a2a41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74e5d297-5752-400b-b3be-940551a89a7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89082204-b76c-4f43-9cb8-3462dd7df4d9");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b241357-65be-45a4-8015-4e3306f8515b", "58d8b6ba-9803-4b75-b47d-6c963e8beebc", "garagestaff", "GARAGESTAFF" },
                    { "518b45d7-60a8-4633-9bf9-c39d757699e8", "b38fece3-ab08-4d9e-aebf-8aa8b97aabbc", "technicalsupport", "TECHNICALSUPPORT" },
                    { "56f5ee85-57d2-44ca-81ab-559e63e4d90f", "d2fef186-fe47-4752-8c07-14caa5dae5e6", "garageadmin", "GARAGEADMIN" },
                    { "b516fb71-4a72-4166-bf21-a40a513a57da", "ad249a00-0891-4690-9475-aad1ef9437cc", "driver", "DRIVER" },
                    { "fd59746b-d4c6-4504-baaa-2666aa21c54a", "62c6ff59-dc36-4b92-bf82-51ab866ece0d", "customerservice", "CUSTOMERSERVICE" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Drivers_DriverID",
                table: "Reports",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Drivers_DriverID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b241357-65be-45a4-8015-4e3306f8515b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "518b45d7-60a8-4633-9bf9-c39d757699e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56f5ee85-57d2-44ca-81ab-559e63e4d90f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b516fb71-4a72-4166-bf21-a40a513a57da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd59746b-d4c6-4504-baaa-2666aa21c54a");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03db5abf-2a40-427d-9909-1eb555b0168c", "5b94aabc-ad65-4ecc-b902-0c8fc3281c32", "customerservice", "CUSTOMERSERVICE" },
                    { "4aea9276-7db6-4441-a2bc-54cd9251530b", "87853da8-4e78-4634-ad8e-7c83790359f3", "garagestaff", "GARAGESTAFF" },
                    { "58abd4cb-ef6f-4488-ad73-5e5c944a2a41", "4cc38ae6-39cb-49da-b5db-9d357449412a", "technicalsupport", "TECHNICALSUPPORT" },
                    { "74e5d297-5752-400b-b3be-940551a89a7c", "1daa0286-85ae-4dd4-add1-3fa466287ecd", "garageadmin", "GARAGEADMIN" },
                    { "89082204-b76c-4f43-9cb8-3462dd7df4d9", "cf3291a4-3033-4b17-82bd-8db614fc32a6", "driver", "DRIVER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Drivers_DriverID",
                table: "Reports",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
