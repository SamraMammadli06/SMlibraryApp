using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Infrastructure.Data;

public class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Log> Logs { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}