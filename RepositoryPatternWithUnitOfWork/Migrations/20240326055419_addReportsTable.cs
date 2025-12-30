using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class addReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01b41d5c-f744-47f4-8c45-0f153c9c5eea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb1d86ac-6268-4996-b30e-53c4d89b8deb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bea3495a-9087-4a10-a010-12f219c663be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec63381f-fca3-4c8a-bf10-948f081ac781");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd5bbee8-5e00-4715-ae82-ef3942f011fe");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFixed = table.Column<bool>(type: "bit", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportMessege = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerServiceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_CustomerServices_CustomerServiceID",
                        column: x => x.CustomerServiceID,
                        principalTable: "CustomerServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reports_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b573309-aa26-4e56-acc4-4d6e1cddeabf", "3bb182f6-9a8c-431b-bc46-35cdac0f19f7", "driver", "DRIVER" },
                    { "9634ec41-ee97-4efd-8058-9d37ebb08c71", "b166006c-43e6-474c-b502-45ee7ade582d", "technicalsupport", "TECHNICALSUPPORT" },
                    { "d1bace03-84f2-4ae2-99b2-fa1607b77c3f", "9177ac34-1c7c-4e09-85ee-387d13c5066c", "garagestaff", "GARAGESTAFF" },
                    { "f9c2ee63-bcfb-4932-9860-c5323e8e1bf5", "aefe6ae7-5e93-41a8-868e-1c8525ac11cd", "garageadmin", "GARAGEADMIN" },
                    { "ffaab369-d825-491f-8834-213d078f93ca", "73900453-47c9-4d36-8201-b53a22bdaceb", "customerservice", "CUSTOMERSERVICE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CustomerServiceID",
                table: "Reports",
                column: "CustomerServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DriverID",
                table: "Reports",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_EmployeeId",
                table: "Reports",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b573309-aa26-4e56-acc4-4d6e1cddeabf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9634ec41-ee97-4efd-8058-9d37ebb08c71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1bace03-84f2-4ae2-99b2-fa1607b77c3f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9c2ee63-bcfb-4932-9860-c5323e8e1bf5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffaab369-d825-491f-8834-213d078f93ca");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01b41d5c-f744-47f4-8c45-0f153c9c5eea", "1a712cd4-f049-41ce-9fc3-122190f25a6c", "garageadmin", "GARAGEADMIN" },
                    { "bb1d86ac-6268-4996-b30e-53c4d89b8deb", "b5a730ef-133a-4de7-944c-50553fdef282", "technicalsupport", "TECHNICALSUPPORT" },
                    { "bea3495a-9087-4a10-a010-12f219c663be", "6cb24bbb-3a67-4680-b76f-ee3c222fdfb9", "customerservice", "CUSTOMERSERVICE" },
                    { "ec63381f-fca3-4c8a-bf10-948f081ac781", "70cf7e7c-4428-4219-b00e-fc78a9bc1ac4", "driver", "DRIVER" },
                    { "fd5bbee8-5e00-4715-ae82-ef3942f011fe", "5f500027-fee8-4f65-9293-b40d2d1abe11", "garagestaff", "GARAGESTAFF" }
                });
        }
    }
}
