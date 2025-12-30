using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class report2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ReportReceiver",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerServiceId",
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
                    { "40ad3c52-11f4-4b9a-abd6-4259944b92e6", "6e345d9f-232d-4a7c-8cac-6836198d44f6", "technicalsupport", "TECHNICALSUPPORT" },
                    { "45659f9b-0367-4631-b609-29eb52e636d4", "5d58c9bf-4c3b-4495-a53b-6d8169842ce3", "customerservice", "CUSTOMERSERVICE" },
                    { "4a252f6d-904e-43aa-8586-c67a2e623784", "bcaa24c8-4eca-4f00-87f9-89d2949a19f9", "driver", "DRIVER" },
                    { "60543968-f70c-477a-a60d-a04aceb47673", "9a469581-2c66-4cee-90bc-be7d636c81d7", "garageadmin", "GARAGEADMIN" },
                    { "fa4e9f39-8b81-4f64-b8f2-9139a0fe6d21", "8d56a160-f237-4172-ae14-3f6239ae8ce8", "garagestaff", "GARAGESTAFF" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceId",
                table: "Reports",
                column: "CustomerServiceId",
                principalTable: "CustomerServices",
                principalColumn: "Id");
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
                keyValue: "40ad3c52-11f4-4b9a-abd6-4259944b92e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45659f9b-0367-4631-b609-29eb52e636d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a252f6d-904e-43aa-8586-c67a2e623784");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60543968-f70c-477a-a60d-a04aceb47673");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa4e9f39-8b81-4f64-b8f2-9139a0fe6d21");

            migrationBuilder.AlterColumn<string>(
                name: "ReportReceiver",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerServiceId",
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
    }
}
