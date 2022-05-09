using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    public partial class CopyCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ce41a814-18f2-47d6-b16e-b2263ad13d43", "89782a8d-8591-441d-a818-46cbfa582d92" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce41a814-18f2-47d6-b16e-b2263ad13d43");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89782a8d-8591-441d-a818-46cbfa582d92");

            migrationBuilder.CreateTable(
                name: "Copy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Copy_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "14976c8a-e19b-4982-b395-ab0dca5efa99", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0b948a1f-c552-41af-9818-77ab56a8be88", 0, "830b551d-43aa-4b14-a07e-97888183e83b", "admin@gmail.com", false, false, null, null, "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEAYBn9fCQDd/h+pOkR6pSruwqRYIRapiqRA0O0TtBR1HPDn+OPgHNU43v0/GJtE/DQ==", null, false, "0cc8e6b5-45b8-4ac8-8b85-c93033f3e689", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "14976c8a-e19b-4982-b395-ab0dca5efa99", "0b948a1f-c552-41af-9818-77ab56a8be88" });

            migrationBuilder.CreateIndex(
                name: "IX_Copy_BookId",
                table: "Copy",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Copy");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "14976c8a-e19b-4982-b395-ab0dca5efa99", "0b948a1f-c552-41af-9818-77ab56a8be88" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14976c8a-e19b-4982-b395-ab0dca5efa99");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ce41a814-18f2-47d6-b16e-b2263ad13d43", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "89782a8d-8591-441d-a818-46cbfa582d92", 0, "8cc98c97-9522-4454-8f2f-3a0380f72451", "admin@gmail.com", false, false, null, null, "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEHpdHsPgUJ23RDxRPshNSTAwpCXFwqC8zIKqBAvvVLBvEMqf586+K3r5uFr75s+rwA==", null, false, "f078d4e5-a123-4155-90f7-5034aaff3a44", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ce41a814-18f2-47d6-b16e-b2263ad13d43", "89782a8d-8591-441d-a818-46cbfa582d92" });
        }
    }
}
