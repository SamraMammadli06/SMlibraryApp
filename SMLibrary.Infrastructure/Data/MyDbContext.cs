using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SMLibrary.Core.Models;
using SMlibraryApp.Core.Models;
using SMLibraryApp.Core.Models;

namespace SMlibraryApp.Infrastructure.Data;

public class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<BookUserName> BookUserNames { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<UserCustomUser> UserCustomUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookUserName>()
            .HasKey(bun => new { bun.BookUserNameId });

        modelBuilder.Entity<BookUserName>()
            .HasOne(bun => bun.Book)
            .WithMany(b => b.UserNames)
            .HasForeignKey(bun => bun.BookId);
         modelBuilder.Entity<Comment>()
            .HasKey(bun => new { bun.Id });

        modelBuilder.Entity<Comment>()
            .HasOne(bun => bun.Book)
            .WithMany(b => b.Comments)
            .HasForeignKey(bun => bun.BookId);
    }
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

}