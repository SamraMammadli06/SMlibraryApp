using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMlibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SMLibrary.Infrastructure.Extensions;
public static class DbContextExtension
{
    public static void ExDbContext(this IServiceCollection serviceCollection, IConfiguration configuration, Assembly Assemblyname)
    {
        serviceCollection.AddDbContext<MyDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("LibraryDb");
            options.UseSqlServer(connectionString, useSqlOptions =>
            {
                useSqlOptions.MigrationsAssembly(Assemblyname.FullName);
            });
        });

        serviceCollection.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
        })
            .AddEntityFrameworkStores<MyDbContext>();
    }

}
