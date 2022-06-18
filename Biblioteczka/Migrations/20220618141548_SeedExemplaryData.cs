using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteczka.Migrations
{
    public partial class SeedExemplaryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9b48eea-7a45-4ae3-aff0-68667143a7f3", "AQAAAAEAACcQAAAAEGcqSKoNUIhzHlTIkHbkU5g8rZn94TojnIQBtu51BuS9aJJwhWGgs/igJdmtI56BoQ==", "be537e94-cfbe-40d7-947a-0e217cf49c07" });

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "Id", "Date", "LastName", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7836), "Witkowski", "Błażej", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 2, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7841), "King", "Stephen", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 3, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7843), "Rowling", "J. K.", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 4, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7845), "Tolkien", "J. R. R.", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 5, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7847), "Clare", "Cassandra", "0b948a1f-c552-41af-9818-77ab56a8be88" }
                });

            migrationBuilder.InsertData(
                table: "Copy",
                columns: new[] { "Id", "BookId", "Date", "Number", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7916), 37485940, "1", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 2, 2, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7919), 37345120, "1", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 3, 5, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7921), 28347123, "1", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 4, 1, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7923), 22745123, "1", "0b948a1f-c552-41af-9818-77ab56a8be88" },
                    { 5, 1, new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7925), 12647189, "1", "0b948a1f-c552-41af-9818-77ab56a8be88" }
                });

            migrationBuilder.UpdateData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7686));

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "City", "Date", "Description", "ISBN", "Image", "IssueNumber", "Pages", "Publisher", "Series", "Title", "UserId", "Year" },
                values: new object[,]
                {
                    { 1, 1, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7880), "Darmowy program graficzny GIMP jest fantastycznym narzędziem do rysowania i tworzenia projektów - o ile potrafisz właściwie i z pomysłem wykorzystać jego możliwości. Wypracowywanie własnych ciekawych rozwiązań wymaga jednak mnóstwo czasu, a porady i triki proponowane w popularnych internetowych poradnikach nie zapewniają spektakularnych efektów. Jeśli chcesz szybko i bezproblemowo podnieść jakość Twoich prac, a dodatkowo zapewnić sobie sporą porcję praktycznej, przydatnej na co dzień wiedzy, sięgnij po tę książkę. Znajdziesz w niej mnóstwo rzetelnych informacji, których zastosowanie pozwoli Ci wzbudzić zachwyt wśród odbiorców Twoich dzieł.", "978-83-283-6236-9", "6f510fcb-0d31-41dd-8008-9b4df4f536bb.jpg", null, 360, "Helion", null, "GIMP. Niesamowite efekty", "0b948a1f-c552-41af-9818-77ab56a8be88", 2019 },
                    { 2, 2, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7883), "Bestialska zbrodnia. Śledztwo pełne znaków zapytania. Stephen King, znajdujący się w szczególnie owocnym okresie twórczości, przedstawia jedną ze swoich najbardziej niepokojących i wciągających opowieści.", "9788381690805 ", "d13e023c-b665-498f-a623-b690f67dd36f.jpg", null, 640, "Prószyński i S-ka", null, "The Outsider", "0b948a1f-c552-41af-9818-77ab56a8be88", 2018 },
                    { 3, 3, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7886), "", "9788380082137 ", "da096239-6bc5-4acc-b4f9-df0a7120aabc.jpg", null, 368, "Media Rodzina", "Harry Potter", "Harry Potter i Komnata Tajemnic", "0b948a1f-c552-41af-9818-77ab56a8be88", 1998 },
                    { 4, 4, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7889), "Hobbit to istota większa od liliputa, mniejsza jednak od krasnala. Fantastyczny, przemyślany do najdrobniejszych szczegółów świat z powieści Tolkiena jest również jego osobistym tworem, a pod barwną fasadą nietrudno się dopatrzyć głębszego sensu i pewnych analogii do współczesności.", "8320716810 ", "d466491d-565c-45e0-a1f2-c3ff722efe41.jpg", null, 320, "Iskry", null, "Hobbit", "0b948a1f-c552-41af-9818-77ab56a8be88", 1960 },
                    { 5, 5, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7893), "Kto przejdzie na stronę mroku? Kogo dopadnie miłość, a czyj związek nie przetrwa próby czasu? I kto zdradzi wszystko, w co do tej pory wierzył?", "9788366409323", "2bf0745b-95cb-464c-9dd1-f8f34534d0a3.jpg", null, 443, "Mag", null, "City of Fallen Angels", "0b948a1f-c552-41af-9818-77ab56a8be88", 2011 },
                    { 6, 2, "Brak", new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7895), "Najbardziej przerażająca powieść króla grozy. Doceniona przez miliony czytelników na całym świecie. Ciebie też porwie.", "9788381257022", "398d6e6d-f7f1-465c-aef7-295e5a72f367.jpg", null, 1104, "Albatros", null, "It (To)", "0b948a1f-c552-41af-9818-77ab56a8be88", 1993 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Copy",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Copy",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Copy",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Copy",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Copy",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b948a1f-c552-41af-9818-77ab56a8be88",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47118b5d-400f-46bd-8d31-f48b4d1f82a2", "AQAAAAEAACcQAAAAEDE36S6n49rH5JrKNGD0mzXgKH2I2kT2MCt/4loEz8RiUSqUWAjB1xwow5vnWs33yQ==", "f9e50513-84b4-4bb4-a1cc-7c3540e4d269" });

            migrationBuilder.UpdateData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2022, 6, 3, 20, 27, 21, 654, DateTimeKind.Local).AddTicks(2205));
        }
    }
}
