using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    public partial class LoanReservationCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserBorrowingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserBorrowingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ce06fc41-a6bc-46f9-b82b-5af6c35ff1d9", "AQAAAAEAACcQAAAAEDyjbMniwek8TtFlxRVnb83mv/qlzeLDGHyNdEQP/Mk7lhwgwO2QfDfkLHoB2YDs7A==", "060ffc6b-a7ee-47dd-b031-4552431afa3a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "830b551d-43aa-4b14-a07e-97888183e83b", "AQAAAAEAACcQAAAAEAYBn9fCQDd/h+pOkR6pSruwqRYIRapiqRA0O0TtBR1HPDn+OPgHNU43v0/GJtE/DQ==", "0cc8e6b5-45b8-4ac8-8b85-c93033f3e689" });
        }
    }
}
