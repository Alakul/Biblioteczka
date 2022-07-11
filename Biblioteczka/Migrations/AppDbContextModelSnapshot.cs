﻿// <auto-generated />
using System;
using Biblioteczka.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Biblioteczka.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Biblioteczka.Areas.Identity.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a9b48eea-7a45-4ae3-aff0-68667143a7f3",
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEGcqSKoNUIhzHlTIkHbkU5g8rZn94TojnIQBtu51BuS9aJJwhWGgs/igJdmtI56BoQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "be537e94-cfbe-40d7-947a-0e217cf49c07",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Biblioteczka.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Author");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7836),
                            LastName = "Witkowski",
                            Name = "Błażej",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7841),
                            LastName = "King",
                            Name = "Stephen",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7843),
                            LastName = "Rowling",
                            Name = "J. K.",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 4,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7845),
                            LastName = "Tolkien",
                            Name = "J. R. R.",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 5,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7847),
                            LastName = "Clare",
                            Name = "Cassandra",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        });
                });

            modelBuilder.Entity("Biblioteczka.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Pages")
                        .HasColumnType("int");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Series")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Book");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7880),
                            Description = "Darmowy program graficzny GIMP jest fantastycznym narzędziem do rysowania i tworzenia projektów - o ile potrafisz właściwie i z pomysłem wykorzystać jego możliwości. Wypracowywanie własnych ciekawych rozwiązań wymaga jednak mnóstwo czasu, a porady i triki proponowane w popularnych internetowych poradnikach nie zapewniają spektakularnych efektów. Jeśli chcesz szybko i bezproblemowo podnieść jakość Twoich prac, a dodatkowo zapewnić sobie sporą porcję praktycznej, przydatnej na co dzień wiedzy, sięgnij po tę książkę. Znajdziesz w niej mnóstwo rzetelnych informacji, których zastosowanie pozwoli Ci wzbudzić zachwyt wśród odbiorców Twoich dzieł.",
                            ISBN = "978-83-283-6236-9",
                            Image = "6f510fcb-0d31-41dd-8008-9b4df4f536bb.jpg",
                            Pages = 360,
                            Publisher = "Helion",
                            Title = "GIMP. Niesamowite efekty",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 2019
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7883),
                            Description = "Bestialska zbrodnia. Śledztwo pełne znaków zapytania. Stephen King, znajdujący się w szczególnie owocnym okresie twórczości, przedstawia jedną ze swoich najbardziej niepokojących i wciągających opowieści.",
                            ISBN = "9788381690805 ",
                            Image = "d13e023c-b665-498f-a623-b690f67dd36f.jpg",
                            Pages = 640,
                            Publisher = "Prószyński i S-ka",
                            Title = "The Outsider",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 2018
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 3,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7886),
                            Description = "",
                            ISBN = "9788380082137 ",
                            Image = "da096239-6bc5-4acc-b4f9-df0a7120aabc.jpg",
                            Pages = 368,
                            Publisher = "Media Rodzina",
                            Series = "Harry Potter",
                            Title = "Harry Potter i Komnata Tajemnic",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 1998
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 4,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7889),
                            Description = "Hobbit to istota większa od liliputa, mniejsza jednak od krasnala. Fantastyczny, przemyślany do najdrobniejszych szczegółów świat z powieści Tolkiena jest również jego osobistym tworem, a pod barwną fasadą nietrudno się dopatrzyć głębszego sensu i pewnych analogii do współczesności.",
                            ISBN = "8320716810 ",
                            Image = "d466491d-565c-45e0-a1f2-c3ff722efe41.jpg",
                            Pages = 320,
                            Publisher = "Iskry",
                            Title = "Hobbit",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 1960
                        },
                        new
                        {
                            Id = 5,
                            AuthorId = 5,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7893),
                            Description = "Kto przejdzie na stronę mroku? Kogo dopadnie miłość, a czyj związek nie przetrwa próby czasu? I kto zdradzi wszystko, w co do tej pory wierzył?",
                            ISBN = "9788366409323",
                            Image = "2bf0745b-95cb-464c-9dd1-f8f34534d0a3.jpg",
                            Pages = 443,
                            Publisher = "Mag",
                            Title = "City of Fallen Angels",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 2011
                        },
                        new
                        {
                            Id = 6,
                            AuthorId = 2,
                            City = "Brak",
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7895),
                            Description = "Najbardziej przerażająca powieść króla grozy. Doceniona przez miliony czytelników na całym świecie. Ciebie też porwie.",
                            ISBN = "9788381257022",
                            Image = "398d6e6d-f7f1-465c-aef7-295e5a72f367.jpg",
                            Pages = 1104,
                            Publisher = "Albatros",
                            Title = "It (To)",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            Year = 1993
                        });
                });

            modelBuilder.Entity("Biblioteczka.Models.Copy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Copy");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7916),
                            Number = 37485940,
                            Status = "1",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7919),
                            Number = 37345120,
                            Status = "1",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 3,
                            BookId = 5,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7921),
                            Number = 28347123,
                            Status = "1",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 4,
                            BookId = 1,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7923),
                            Number = 22745123,
                            Status = "1",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        },
                        new
                        {
                            Id = 5,
                            BookId = 1,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7925),
                            Number = 12647189,
                            Status = "1",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        });
                });

            modelBuilder.Entity("Biblioteczka.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CopyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoanDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserBorrowingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Loan");
                });

            modelBuilder.Entity("Biblioteczka.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LibraryCardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pesel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profile");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(2022, 6, 18, 16, 15, 48, 384, DateTimeKind.Local).AddTicks(7686),
                            LastName = "Min",
                            LibraryCardNumber = "LCN12345678",
                            Name = "Ad",
                            Pesel = "12345678901",
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88"
                        });
                });

            modelBuilder.Entity("Biblioteczka.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CopyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserBorrowingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "14976c8a-e19b-4982-b395-ab0dca5efa99",
                            ConcurrencyStamp = "1",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "14976c8a-e19e-4982-b395-ab0dca5efa98",
                            ConcurrencyStamp = "1",
                            Name = "Użytkownik",
                            NormalizedName = "UŻYTKOWNIK"
                        },
                        new
                        {
                            Id = "14936c8a-e19e-4982-b395-ab0dca5efa97",
                            ConcurrencyStamp = "1",
                            Name = "Bibliotekarz",
                            NormalizedName = "BIBLIOTEKARZ"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                            RoleId = "14976c8a-e19b-4982-b395-ab0dca5efa99"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Biblioteczka.Models.Book", b =>
                {
                    b.HasOne("Biblioteczka.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Biblioteczka.Areas.Identity.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Biblioteczka.Areas.Identity.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteczka.Areas.Identity.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Biblioteczka.Areas.Identity.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
