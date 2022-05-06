using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    public partial class BookAuthorEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Book");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Book",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Author",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Author",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_UserId",
                table: "Book",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_UserId",
                table: "Author",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_AspNetUsers_UserId",
                table: "Author",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_AspNetUsers_UserId",
                table: "Book",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_AspNetUsers_UserId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_AspNetUsers_UserId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_UserId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Author_UserId",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Author");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
