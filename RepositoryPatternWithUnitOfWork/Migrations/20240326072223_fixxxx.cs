using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class fixxxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceID",
                table: "Reports");

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

            migrationBuilder.RenameColumn(
                name: "CustomerServiceID",
                table: "Reports",
                newName: "CustomerServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_CustomerServiceID",
                table: "Reports",
                newName: "IX_Reports_CustomerServiceId");

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
                    { "03db5abf-2a40-427d-9909-1eb555b0168c", "5b94aabc-ad65-4ecc-b902-0c8fc3281c32", "customerservice", "CUSTOMERSERVICE" },
                    { "4aea9276-7db6-4441-a2bc-54cd9251530b", "87853da8-4e78-4634-ad8e-7c83790359f3", "garagestaff", "GARAGESTAFF" },
                    { "58abd4cb-ef6f-4488-ad73-5e5c944a2a41", "4cc38ae6-39cb-49da-b5db-9d357449412a", "technicalsupport", "TECHNICALSUPPORT" },
                    { "74e5d297-5752-400b-b3be-940551a89a7c", "1daa0286-85ae-4dd4-add1-3fa466287ecd", "garageadmin", "GARAGEADMIN" },
                    { "89082204-b76c-4f43-9cb8-3462dd7df4d9", "cf3291a4-3033-4b17-82bd-8db614fc32a6", "driver", "DRIVER" }
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

            migrationBuilder.RenameColumn(
                name: "CustomerServiceId",
                table: "Reports",
                newName: "CustomerServiceID");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_CustomerServiceId",
                table: "Reports",
                newName: "IX_Reports_CustomerServiceID");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerServiceID",
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
                    { "2b573309-aa26-4e56-acc4-4d6e1cddeabf", "3bb182f6-9a8c-431b-bc46-35cdac0f19f7", "driver", "DRIVER" },
                    { "9634ec41-ee97-4efd-8058-9d37ebb08c71", "b166006c-43e6-474c-b502-45ee7ade582d", "technicalsupport", "TECHNICALSUPPORT" },
                    { "d1bace03-84f2-4ae2-99b2-fa1607b77c3f", "9177ac34-1c7c-4e09-85ee-387d13c5066c", "garagestaff", "GARAGESTAFF" },
                    { "f9c2ee63-bcfb-4932-9860-c5323e8e1bf5", "aefe6ae7-5e93-41a8-868e-1c8525ac11cd", "garageadmin", "GARAGEADMIN" },
                    { "ffaab369-d825-491f-8834-213d078f93ca", "73900453-47c9-4d36-8201-b53a22bdaceb", "customerservice", "CUSTOMERSERVICE" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_CustomerServices_CustomerServiceID",
                table: "Reports",
                column: "CustomerServiceID",
                principalTable: "CustomerServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
