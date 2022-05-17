﻿using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteczka.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Copy> Copy { get; set; }
    public DbSet<Loan> Loan { get; set; }
    public DbSet<Reservation> Reservation { get; set; }

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
            Id = "14976c8a-e19b-4982-b395-ab0dca5efa99",
            Name = "Admin",
            NormalizedName = "Admin",
            ConcurrencyStamp = "1"
        };
        builder.Entity<IdentityRole>().HasData(identityRole);

        AppUser identityUser = new AppUser()
        {
            Id = "0b948a1f-c552-41af-9818-77ab56a8be88",
            UserName = "admin@gmail.com",
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
    }
}