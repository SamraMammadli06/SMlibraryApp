using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMLibrary.Core.Models;
using SMlibraryApp.Core.Models;
using SMLibraryApp.Core.Models;

namespace SMlibraryApp.Infrastructure.Data;

public class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<BookUserName> BookUserNames { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookUserName>()
            .HasKey(bun => new { bun.BookUserNameId });

        modelBuilder.Entity<BookUserName>()
            .HasOne(bun => bun.Book)
            .WithMany(b => b.UserNames)
            .HasForeignKey(bun => bun.BookId);
    }
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

}