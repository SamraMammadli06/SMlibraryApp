using SMlibraryApp.Presentation.Middlewares;
using SMLibrary.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection();
builder.Services.AddAuthorization();
builder.Services.ExRepositories();
builder.Services.ExDbContext(builder.Configuration, System.Reflection.Assembly.GetExecutingAssembly());
builder.Services.ExServices(builder.Configuration);
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
