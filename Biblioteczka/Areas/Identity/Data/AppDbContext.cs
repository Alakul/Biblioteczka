using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteczka.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Copy> Copy { get; set; }
    public DbSet<Loan> Loan { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<Profile> Profile { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        SeedAdmin(builder);
        SeedRoles(builder);

        SeedAuhors(builder);
        SeedBooks(builder);
        SeedCopies(builder);
    }

    private void SeedAdmin(ModelBuilder builder)
    {
        IdentityRole identityRole = new IdentityRole()
        {
            Id = "14976c8a-e19b-4982-b395-ab0dca5efa99",
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            ConcurrencyStamp = "1"
        };
        builder.Entity<IdentityRole>().HasData(identityRole);

        AppUser identityUser = new AppUser()
        {
            Id = "0b948a1f-c552-41af-9818-77ab56a8be88",
            UserName = "admin",
            Email = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM"
        };
        var hasher = new PasswordHasher<AppUser>();
        identityUser.PasswordHash = hasher.HashPassword(identityUser, "12345678");
        builder.Entity<AppUser>().HasData(identityUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = identityRole.Id,
            UserId = identityUser.Id
        });

        Profile profile = new Profile()
        {
            Id = 1,
            UserId = identityUser.Id,
            Name = "Ad",
            LastName = "Min",
            Pesel = "12345678901",
            LibraryCardNumber = "LCN12345678",
            Date = DateTime.Now,
        };
        builder.Entity<Profile>().HasData(profile);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole {
                Id = "14976c8a-e19e-4982-b395-ab0dca5efa98",
                Name = "Użytkownik",
                NormalizedName = "UŻYTKOWNIK",
                ConcurrencyStamp = "1"
            },
            new IdentityRole {
                Id = "14936c8a-e19e-4982-b395-ab0dca5efa97",
                Name = "Bibliotekarz",
                NormalizedName = "BIBLIOTEKARZ",
                ConcurrencyStamp = "1"
            }
        );
    }

    private void SeedAuhors(ModelBuilder builder)
    {
        builder.Entity<Author>().HasData(
            new Author { Id = 1, UserId= "0b948a1f-c552-41af-9818-77ab56a8be88", Name = "Błażej", LastName = "Witkowski", Date = DateTime.Now },
            new Author { Id = 2, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", Name = "Stephen", LastName = "King", Date = DateTime.Now },
            new Author { Id = 3, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", Name = "J. K.", LastName = "Rowling", Date = DateTime.Now },
            new Author { Id = 4, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", Name = "J. R. R.", LastName = "Tolkien", Date = DateTime.Now },
            new Author { Id = 5 , UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", Name = "Cassandra", LastName = "Clare", Date = DateTime.Now }
        );
    }
    private void SeedBooks(ModelBuilder builder)
    {
        builder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                AuthorId = 1,
                Title = "GIMP. Niesamowite efekty",
                Description = "Darmowy program graficzny GIMP jest fantastycznym narzędziem do rysowania i tworzenia projektów - o ile potrafisz właściwie i z pomysłem wykorzystać jego możliwości. Wypracowywanie własnych ciekawych rozwiązań wymaga jednak mnóstwo czasu, a porady i triki proponowane w popularnych internetowych poradnikach nie zapewniają spektakularnych efektów. Jeśli chcesz szybko i bezproblemowo podnieść jakość Twoich prac, a dodatkowo zapewnić sobie sporą porcję praktycznej, przydatnej na co dzień wiedzy, sięgnij po tę książkę. Znajdziesz w niej mnóstwo rzetelnych informacji, których zastosowanie pozwoli Ci wzbudzić zachwyt wśród odbiorców Twoich dzieł.",
                Image = "6f510fcb-0d31-41dd-8008-9b4df4f536bb.jpg",
                Year = 2019,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "978-83-283-6236-9",
                Pages = 360,
                Publisher = "Helion",
                Date = DateTime.Now,
            },
            new Book {
                Id = 2,
                AuthorId = 2,
                Title = "The Outsider",
                Description = "Bestialska zbrodnia. Śledztwo pełne znaków zapytania. Stephen King, znajdujący się w szczególnie owocnym okresie twórczości, przedstawia jedną ze swoich najbardziej niepokojących i wciągających opowieści.",
                Image = "d13e023c-b665-498f-a623-b690f67dd36f.jpg",
                Year = 2018,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "9788381690805 ",
                Pages = 640,
                Publisher = "Prószyński i S-ka",
                Date = DateTime.Now,
            },
            new Book {
                Id = 3,
                AuthorId = 3,
                Title = "Harry Potter i Komnata Tajemnic",
                Description = "",
                Image = "da096239-6bc5-4acc-b4f9-df0a7120aabc.jpg",
                Year = 1998,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "9788380082137 ",
                Pages = 368,
                Publisher = "Media Rodzina",
                Date = DateTime.Now,
                Series = "Harry Potter"
            },
            new Book {
                Id = 4,
                AuthorId = 4,
                Title = "Hobbit",
                Description = "Hobbit to istota większa od liliputa, mniejsza jednak od krasnala. Fantastyczny, przemyślany do najdrobniejszych szczegółów świat z powieści Tolkiena jest również jego osobistym tworem, a pod barwną fasadą nietrudno się dopatrzyć głębszego sensu i pewnych analogii do współczesności.",
                Image = "d466491d-565c-45e0-a1f2-c3ff722efe41.jpg",
                Year = 1960,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "8320716810 ",
                Pages = 320,
                Publisher = "Iskry",
                Date = DateTime.Now,
            },
            new Book {
                Id = 5,
                AuthorId = 5,
                Title = "City of Fallen Angels",
                Description = "Kto przejdzie na stronę mroku? Kogo dopadnie miłość, a czyj związek nie przetrwa próby czasu? I kto zdradzi wszystko, w co do tej pory wierzył?",
                Image = "2bf0745b-95cb-464c-9dd1-f8f34534d0a3.jpg",
                Year = 2011,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "9788366409323",
                Pages = 443,
                Publisher = "Mag",
                Date = DateTime.Now,
            },
            new Book
            {
                Id = 6,
                AuthorId = 2,
                Title = "It (To)",
                Description = "Najbardziej przerażająca powieść króla grozy. Doceniona przez miliony czytelników na całym świecie. Ciebie też porwie.",
                Image = "398d6e6d-f7f1-465c-aef7-295e5a72f367.jpg",
                Year = 1993,
                City = "Brak",
                UserId = "0b948a1f-c552-41af-9818-77ab56a8be88",
                ISBN = "9788381257022",
                Pages = 1104,
                Publisher = "Albatros",
                Date = DateTime.Now,
            }
        );

    }
    private void SeedCopies(ModelBuilder builder)
    {
        builder.Entity<Copy>().HasData(
            new Copy { Id = 1, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", BookId = 1, Number = 37485940, Status = "1", Date = DateTime.Now },
            new Copy { Id = 2, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", BookId = 2, Number = 37345120, Status = "1", Date = DateTime.Now },
            new Copy { Id = 3, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", BookId = 5, Number = 28347123, Status = "1", Date = DateTime.Now },
            new Copy { Id = 4, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", BookId = 1, Number = 22745123, Status = "1", Date = DateTime.Now },
            new Copy { Id = 5, UserId = "0b948a1f-c552-41af-9818-77ab56a8be88", BookId = 1, Number = 12647189, Status = "1", Date = DateTime.Now }
        );
    }
}
