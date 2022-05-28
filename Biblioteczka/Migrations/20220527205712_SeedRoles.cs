using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteczka.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14976c8a-e19b-4982-b395-ab0dca5efa99",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14936c8a-e19e-4982-b395-ab0dca5efa97", "1", "Bibliotekarz", "BIBLIOTEKARZ" },
                    { "14976c8a-e19e-4982-b395-ab0dca5efa98", "1", "Użytkownik", "UŻYTKOWNIK" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89f6c359-5a11-41d6-9108-8ee4deaffcfa", "AQAAAAEAACcQAAAAEEKaOJVcgWxLdsV4DXwpoNtSK64PJg3BuvIyp1w0OcaYRLEPyqCnRYCWDdYT2c6f3g==", "c6721404-6c90-4a09-947c-6b8ac3d70e0c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14936c8a-e19e-4982-b395-ab0dca5efa97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14976c8a-e19e-4982-b395-ab0dca5efa98");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14976c8a-e19b-4982-b395-ab0dca5efa99",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "Admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ea19925-304d-4a03-8e70-e26803dd9f7e", "AQAAAAEAACcQAAAAEBIja76xnB632jG57FlqCKM1tOvCxKXyCia3a4CsgWE2Mp0rhLaO0HIJdyUD3MpjNQ==", "4518f76f-426f-4b18-8b9f-116c8c7964e3" });
        }
    }
}
