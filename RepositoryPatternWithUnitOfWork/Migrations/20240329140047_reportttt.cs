using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class reportttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Drivers_DriverID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_DriverID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_EmployeeId",
                table: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d1b31dd-c108-41ac-91e2-e51231d6ca71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20b4b5f9-cf09-4b65-bbfc-5ab45392532e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7331989c-4831-4dab-85f5-5061eafb90ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f6b62fd-a418-447c-9443-26918556576e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8e6135a-c8bb-459c-a499-34aaad35f210");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerServiceId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReporterId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0846f7dc-a082-4f5a-9693-eb670d10983d", "78409234-a057-40cb-a43b-ff79ea95389f", "garageadmin", "GARAGEADMIN" },
                    { "085b7db0-79a1-4d7a-a094-a82a72b8bfe9", "2d4efb1b-2e9d-4ce9-8311-c0e54327b5d0", "customerservice", "CUSTOMERSERVICE" },
                    { "ba4009fe-b926-43fc-9e0f-b8ca39ed1725", "02e23c7e-936c-4c56-b68b-5ee324da41db", "garagestaff", "GARAGESTAFF" },
                    { "d247bb32-af22-4e3a-9761-f505b29be1ce", "76649de6-caaa-41f4-a4d1-4a9011789015", "technicalsupport", "TECHNICALSUPPORT" },
                    { "e2c7610e-5a82-4cdd-a7f0-d54109c690f2", "4b812521-7b2d-4a65-83c8-cad3bbdd6791", "driver", "DRIVER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceId",
                table: "Reports",
                column: "CustomerServiceId",
                principalTable: "CustomerServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceId",
                table: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0846f7dc-a082-4f5a-9693-eb670d10983d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "085b7db0-79a1-4d7a-a094-a82a72b8bfe9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba4009fe-b926-43fc-9e0f-b8ca39ed1725");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d247bb32-af22-4e3a-9761-f505b29be1ce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2c7610e-5a82-4cdd-a7f0-d54109c690f2");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerServiceId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "DriverID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d1b31dd-c108-41ac-91e2-e51231d6ca71", "70b2f7cb-cf79-4357-a33a-ec808ab4edea", "technicalsupport", "TECHNICALSUPPORT" },
                    { "20b4b5f9-cf09-4b65-bbfc-5ab45392532e", "29160aad-4b78-4973-9b06-a6c74021c13d", "garageadmin", "GARAGEADMIN" },
                    { "7331989c-4831-4dab-85f5-5061eafb90ae", "dd7f3819-cec4-4890-a1e8-dfceb7b66e48", "customerservice", "CUSTOMERSERVICE" },
                    { "8f6b62fd-a418-447c-9443-26918556576e", "5f52ff7d-13e2-4dd8-82fb-fb6a7a225989", "garagestaff", "GARAGESTAFF" },
                    { "a8e6135a-c8bb-459c-a499-34aaad35f210", "48a5cef7-fc80-43c4-8c7a-104e91296429", "driver", "DRIVER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DriverID",
                table: "Reports",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_EmployeeId",
                table: "Reports",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceId",
                table: "Reports",
                column: "CustomerServiceId",
                principalTable: "CustomerServices",
                principalColumn: "Id");

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
    }
}
