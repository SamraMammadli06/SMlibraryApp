using SMlibraryApp.Repository.Base;
using SMlibraryApp.Repository;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepository>(provider => {
    const string connectionStringName = "LibraryDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);
    if(string.IsNullOrWhiteSpace(connectionString)) {
        throw new Exception($"{connectionStringName} not found");
    }
    
    return new BooksRepository(connectionString);
});;


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
