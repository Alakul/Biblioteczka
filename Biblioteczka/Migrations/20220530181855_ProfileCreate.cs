using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteczka.Migrations
{
    public partial class ProfileCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copy_Book_BookId",
                table: "Copy");

            migrationBuilder.DropIndex(
                name: "IX_Copy_BookId",
                table: "Copy");

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LibraryCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e121471-7a16-4a01-9b19-5b148e8ce4b2", "AQAAAAEAACcQAAAAEBU2O0yTad5ECtY7m03pQgjcy2jP7JuePdDuoGkK6f+Ygefb+zDs4gYOu6qFTCMzKQ==", "96cadb0f-82d6-4546-84c7-1333c5a119c6" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89f6c359-5a11-41d6-9108-8ee4deaffcfa", "AQAAAAEAACcQAAAAEEKaOJVcgWxLdsV4DXwpoNtSK64PJg3BuvIyp1w0OcaYRLEPyqCnRYCWDdYT2c6f3g==", "c6721404-6c90-4a09-947c-6b8ac3d70e0c" });

            migrationBuilder.CreateIndex(
                name: "IX_Copy_BookId",
                table: "Copy",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Copy_Book_BookId",
                table: "Copy",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
