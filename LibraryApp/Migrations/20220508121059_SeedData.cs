using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
