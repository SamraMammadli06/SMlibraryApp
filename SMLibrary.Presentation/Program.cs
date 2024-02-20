using Microsoft.AspNetCore.Authentication.Cookies;
using SMLibrary.Core.Repository;
using SMLibrary.Infrastructure.Repository;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Core.Services;
using SMlibraryApp.Infrastructure.Repository;
using SMlibraryApp.Infrastructure.Services;
using SMlibraryApp.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDataProtection();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";
    });
builder.Services.AddAuthorization();

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

builder.Services.AddScoped<IUserBooksRepository>(provider =>
{
    const string connectionStringName = "LibraryDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"{connectionStringName} not found");
    }

    return new UserBooksRepository(connectionString);
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

builder.Services.AddTransient<ILogService>(provider =>
{
    bool IsLogEnabled = builder.Configuration.GetSection("IsLogEnabled").Get<bool>();
    return new LogService(IsLogEnabled);
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
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
