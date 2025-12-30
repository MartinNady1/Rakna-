using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class fixxxx3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "ReportReceiver",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ReportReceiver",
                table: "Reports");

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
        }
    }
}
