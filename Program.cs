using SMlibraryApp.Repository.Base;
using SMlibraryApp.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using SMlibraryApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDataProtection();

builder.Services.AddScoped<IBookRepository>(provider =>
{
    const string connectionStringName = "LibraryDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"{connectionStringName} not found");
    }

    return new BooksRepository(connectionString);
});

builder.Services.AddScoped<ILogRepository>(provider =>
{
    const string connectionStringName = "LibraryDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"{connectionStringName} not found");
    }

    return new LogRepository(connectionString);
});

builder.Services.AddScoped<IUserRepository>(provider =>
{
    const string connectionStringName = "LibraryDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"{connectionStringName} not found");
    }

    return new UserRepository(connectionString);
});

builder.Services.AddTransient<LoggingMiddleware>();


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

app.UseMiddleware<LoggingMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Identity}/{action=Login}");

app.Run();
