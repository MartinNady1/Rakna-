using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class park : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DeleteData(
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
                keyValue: "fa4e9f39-8b81-4f64-b8f2-9139a0fe6d21");*/

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "parkingSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

          /*  migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16473c57-bde4-4014-bad9-cd2cfcbcacaf", "632b7c4f-7d6f-47bf-9e7b-bd2794e86353", "customerservice", "CUSTOMERSERVICE" },
                    { "1688c50d-8afa-475a-b1ed-d11c35f34ea2", "85ebd8d9-ce65-4e12-a339-2bc8835e3aa2", "technicalsupport", "TECHNICALSUPPORT" },
                    { "38464ea5-bdf4-4ace-989d-f4909d9ebca6", "b8ded986-4a9d-4ca9-8907-03a877f81e5f", "garageadmin", "GARAGEADMIN" },
                    { "3d1cadcd-cc6b-4d43-a626-37a0f7932f66", "bfed1dfc-603c-4332-b44e-7c8a1801095d", "driver", "DRIVER" },
                    { "b6501e3c-9c71-416a-bea3-52d667cfefe9", "a0336c20-2a24-40c8-8086-37d0cb34ff4d", "garagestaff", "GARAGESTAFF" }
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DeleteData(
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

            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "parkingSessions");

/*            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40ad3c52-11f4-4b9a-abd6-4259944b92e6", "6e345d9f-232d-4a7c-8cac-6836198d44f6", "technicalsupport", "TECHNICALSUPPORT" },
                    { "45659f9b-0367-4631-b609-29eb52e636d4", "5d58c9bf-4c3b-4495-a53b-6d8169842ce3", "customerservice", "CUSTOMERSERVICE" },
                    { "4a252f6d-904e-43aa-8586-c67a2e623784", "bcaa24c8-4eca-4f00-87f9-89d2949a19f9", "driver", "DRIVER" },
                    { "60543968-f70c-477a-a60d-a04aceb47673", "9a469581-2c66-4cee-90bc-be7d636c81d7", "garageadmin", "GARAGEADMIN" },
                    { "fa4e9f39-8b81-4f64-b8f2-9139a0fe6d21", "8d56a160-f237-4172-ae14-3f6239ae8ce8", "garagestaff", "GARAGESTAFF" }
                });*/
        }
    }
}
