using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteczka.Migrations
{
    public partial class SeedProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "592a83fb-5240-4d13-a5b4-4902db8fe2ad", "AQAAAAEAACcQAAAAEKqZjGDi4e5KbaAabupqarocbC9Ql6WgpT6/QND7GAQ681amQMBmvPToIvYzd5UzBg==", "59da1edf-edc7-40b6-925d-e114b7b1a027", "admin" });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "Date", "LastName", "LibraryCardNumber", "Name", "Pesel", "UserId" },
                values: new object[] { 1, new DateTime(2022, 6, 1, 17, 51, 23, 655, DateTimeKind.Local).AddTicks(5463), "Min", "LCN12345678", "Ad", "12345678901", "0b948a1f-c552-41af-9818-77ab56a8be88" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "1e121471-7a16-4a01-9b19-5b148e8ce4b2", "AQAAAAEAACcQAAAAEBU2O0yTad5ECtY7m03pQgjcy2jP7JuePdDuoGkK6f+Ygefb+zDs4gYOu6qFTCMzKQ==", "96cadb0f-82d6-4546-84c7-1333c5a119c6", "admin@gmail.com" });
        }
    }
}
