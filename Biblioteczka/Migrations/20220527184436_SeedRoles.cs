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
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14936c8a-e19e-4982-b395-ab0dca5efa97", "1", "Librarian", "LIBRARIAN" },
                    { "14976c8a-e19e-4982-b395-ab0dca5efa98", "1", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15a30e08-4f0f-47e5-9d96-d8470112d561", "AQAAAAEAACcQAAAAEF1NCbj3k618qa+WGgTjD3PDeZ0l5i/pltrZIhucjA1xbnv2HTl3G/pAAIoej6+iyA==", "39c7851e-db2f-4fde-aa9b-42ebc35af3d5" });
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
                column: "NormalizedName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ea19925-304d-4a03-8e70-e26803dd9f7e", "AQAAAAEAACcQAAAAEBIja76xnB632jG57FlqCKM1tOvCxKXyCia3a4CsgWE2Mp0rhLaO0HIJdyUD3MpjNQ==", "4518f76f-426f-4b18-8b9f-116c8c7964e3" });
        }
    }
}
