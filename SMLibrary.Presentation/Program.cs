using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Core.Services;
using SMlibraryApp.Infrastructure.Repository;
using SMlibraryApp.Infrastructure.Services;
using SMlibraryApp.Infrastructure.Data;
using SMlibraryApp.Presentation.Middlewares;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDataProtection();

    
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IBookRepository,BooksRepository>();
builder.Services.AddScoped<ILogRepository,LogRepository>();

builder.Services.AddTransient<ILogService>(provider =>
{
    bool IsLogEnabled = builder.Configuration.GetSection("IsLogEnabled").Get<bool>();
    return new LogService(IsLogEnabled);
});

builder.Services.AddTransient<LoggingMiddleware>();

builder.Services.AddDbContext<MyDbContext>(dbContextOptionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("LibraryDb");
    dbContextOptionsBuilder.UseSqlServer(connectionString, options =>
    {
        options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    });
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
})
    .AddEntityFrameworkStores<MyDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
