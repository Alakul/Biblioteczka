using LibraryApp.Areas.Identity.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }

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
    }

    private void SeedAdmin(ModelBuilder builder)
    {
        IdentityRole identityRole = new IdentityRole()
        {
            Name = "Admin",
            NormalizedName = "Admin",
            ConcurrencyStamp = "1"
        };
        builder.Entity<IdentityRole>().HasData(identityRole);

        AppUser identityUser = new AppUser()
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
        };
        var hasher = new PasswordHasher<AppUser>();
        identityUser.PasswordHash = hasher.HashPassword(identityUser, "12345678");
        builder.Entity<AppUser>().HasData(identityUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = identityRole.Id,
            UserId = identityUser.Id
        });
    }
}
