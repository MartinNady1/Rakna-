using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rakna.EF.Migrations
{
    /// <inheritdoc />
    public partial class ModelParkingSession2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
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

            migrationBuilder.AddColumn<string>(
                name: "PlateLetters",
                table: "parkingSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumbers",
                table: "parkingSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

          /*  migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1271846b-4a4f-403d-bff9-a648a67ae36a", "d6a3d481-9977-43a0-b9b5-413eebdd6239", "technicalsupport", "TECHNICALSUPPORT" },
                    { "215785b2-e133-427a-84a7-88f1bd9ea665", "b7e22c17-bf51-492f-867b-12713a06e68b", "garagestaff", "GARAGESTAFF" },
                    { "3f3b9857-6287-44b8-ac04-9510bccabc94", "4db59fe9-edaf-411f-aacb-684472bb11d2", "customerservice", "CUSTOMERSERVICE" },
                    { "6e7e9e9b-6afc-4bb9-b6ab-038dccda9a01", "4faaa818-4f88-497e-9f74-f1ed4db4711d", "garageadmin", "GARAGEADMIN" },
                    { "da4de0d2-3719-4437-b50b-a6331ddba47b", "7fcac394-992c-4a0b-bd2e-332fc9f121cb", "driver", "DRIVER" }
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {/*
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1271846b-4a4f-403d-bff9-a648a67ae36a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "215785b2-e133-427a-84a7-88f1bd9ea665");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f3b9857-6287-44b8-ac04-9510bccabc94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e7e9e9b-6afc-4bb9-b6ab-038dccda9a01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da4de0d2-3719-4437-b50b-a6331ddba47b");*/

            migrationBuilder.DropColumn(
                name: "PlateLetters",
                table: "parkingSessions");

            migrationBuilder.DropColumn(
                name: "PlateNumbers",
                table: "parkingSessions");

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
        }
    }
}
